Imports System.Security.Claims
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Web.Http.Controllers
Imports System.Web.Http
Public Class ClaimsAuthorizationAttribute
    Inherits AuthorizeAttribute

    Public Property ClaimTypes As String

    Public Overrides Function OnAuthorizationAsync(actionContext As HttpActionContext, cancellationToken As CancellationToken) As Tasks.Task
        Dim principal As ClaimsPrincipal = CType(actionContext.RequestContext.Principal, ClaimsPrincipal)
        Dim ns As String = My.Settings.RoleNameSpace.ToLower & "role-"
        Dim hasAccess As Boolean = False
        Dim role As enumRole

        If principal.Identity.IsAuthenticated = False Then
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized)
        ElseIf String.IsNullOrEmpty(ClaimTypes) = False Then
            role = RoleProcessor.GetRoleFlags(principal)

            If role <> enumRole.None Then
                For Each c As String In ClaimTypes.Split("|"c, ":"c)
                    If (role And RoleProcessor.GetRoleFlags(c)) <> enumRole.None Then
                        hasAccess = True
                    End If
                Next c
            End If
            If hasAccess = False Then
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized)
            End If
        End If

        Return Tasks.Task.FromResult(Of Object)(Nothing)
    End Function
End Class

