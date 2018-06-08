Imports Autofac
Imports System.Web.Http.Controllers
Imports System.Web.Http.Filters
Public Class MetricsLogAttribute
    Inherits ActionFilterAttribute

    Public Sub New(ByVal objectContainer As IContainer)
        Me.ObjectContainer = objectContainer
    End Sub

    Public Property ObjectContainer As IContainer

    Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
        actionContext.RequestContext.RouteData.Values.Add("adpStartTs", Date.UtcNow)
        MyBase.OnActionExecuting(actionContext)
    End Sub

    Public Overrides Sub OnActionExecuted(actionExecuteContext As HttpActionExecutedContext)
        MyBase.OnActionExecuted(actionExecuteContext)
        Dim startTime As Date = CType(actionExecuteContext.ActionContext.RequestContext.RouteData.Values("adpStartTs"), Date)
        Dim duration As TimeSpan = Date.UtcNow.Subtract(startTime)
        Dim saver As IWebMetricSaver
        Dim attributes As New Dictionary(Of String, String)
        Dim statusCode As String = String.Empty

        attributes.Add("Action", actionExecuteContext.ActionContext.ActionDescriptor.ActionName)

        For Each h As KeyValuePair(Of String, IEnumerable(Of String)) In actionExecuteContext.Request.Headers
            If String.Compare("Authorization", h.Key, True) <> 0 Then
                attributes.Add(h.Key, String.Join(",", h.Value))
            End If
        Next

        If actionExecuteContext.Response IsNot Nothing Then
            statusCode = actionExecuteContext.Response.StatusCode.ToString
        End If

        Using scope As ILifetimeScope = ObjectContainer.BeginLifetimeScope
            saver = scope.Resolve(Of IWebMetricSaver)()
            saver.Create(New Settings(),
                         actionExecuteContext.Request.RequestUri.ToString,
                         actionExecuteContext.Request.Method.ToString,
                         startTime,
                         duration.TotalSeconds,
                         statusCode,
                         actionExecuteContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                         attributes
            )
        End Using
    End Sub
End Class
