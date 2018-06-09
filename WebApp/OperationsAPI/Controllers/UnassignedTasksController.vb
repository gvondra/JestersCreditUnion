Imports Autofac
Imports AutoMapper
Imports System.Net
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    <MetricsLog()>
    Public Class UnassignedTasksController
        Inherits ControllerBase

        Private Shared m_mapperConfiguration As MapperConfiguration

        Shared Sub New()
            m_mapperConfiguration = New MapperConfiguration(
                Sub(exp As IMapperConfigurationExpression)
                    exp.CreateMap(Of IUnassignedTask, UnassignedTask)()
                End Sub
            )
        End Sub

        <HttpGet(), ClaimsAuthorization(ClaimTypes:="TP|TA")> Public Function GetTasks() As IHttpActionResult
            Dim result As IHttpActionResult = Nothing
            Dim user As IUser
            Dim userFactory As JestersCreditUnion.CommonAPI.IUserFactory
            Dim taskFactory As IUnassignedTaskFactory
            Dim tasks As IEnumerable(Of UnassignedTask)
            Dim mapper As IMapper

            Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                userFactory = scope.Resolve(Of JestersCreditUnion.CommonAPI.IUserFactory)()
                user = userFactory.Get(CType(Me.User, ClaimsPrincipal))

                mapper = New Mapper(m_mapperConfiguration)
                taskFactory = scope.Resolve(Of IUnassignedTaskFactory)()
                tasks = From t In taskFactory.GetByUser(New Settings(), user.UserId)
                        Select mapper.Map(Of UnassignedTask)(t)

                result = Ok(tasks)
            End Using

            Return result
        End Function
    End Class
End Namespace