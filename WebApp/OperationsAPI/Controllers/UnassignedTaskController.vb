Imports Autofac
Imports System.Net
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class UnassignedTaskController
        Inherits ControllerBase

        <HttpPut(), ClaimsAuthorization(ClaimTypes:="TP|TA"), Route("api/UnassignedTask/{id}/Claim")>
        Public Function ClaimTask(<FromUri> ByVal id As Guid) As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim taskFactory As ITaskFactory
            Dim innerTask As ITask = Nothing
            Dim userFactory As JestersCreditUnion.CommonAPI.IUserFactory
            Dim user As IUser = Nothing
            Dim task As Task(Of ITask)
            Dim taskSaver As ITaskSaver

            If result Is Nothing AndAlso id.Equals(Guid.Empty) Then
                result = BadRequest("Missing or invalid task id")
            End If

            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                taskFactory = scope.Resolve(Of ITaskFactory)()
                userFactory = scope.Resolve(Of JestersCreditUnion.CommonAPI.IUserFactory)()

                If result Is Nothing Then
                    task = System.Threading.Tasks.Task(Of ITask).Run(Of ITask)(Function() taskFactory.Get(New Settings(), id))
                    user = userFactory.Get(CType(Me.User, ClaimsPrincipal))
                    task.Wait()
                    innerTask = task.Result
                End If

                If innerTask Is Nothing Then
                    result = NotFound()
                End If

                If result Is Nothing Then
                    If innerTask.GetUser(New Settings()) Is Nothing Then
                        innerTask.SetUser(user)
                        taskSaver = scope.Resolve(Of ITaskSaver)()
                        taskSaver.Update(New Settings(), innerTask)
                        result = Ok("Task claimed successfuly")
                    Else
                        result = BadRequest("This task has been assigned previously")
                    End If
                End If
            End Using

            Return result
        End Function
    End Class
End Namespace