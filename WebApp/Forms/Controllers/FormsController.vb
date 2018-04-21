Imports Autofac
Imports System.Net
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    Public Class FormsController
        Inherits ControllerBase

        <HttpPost(), Route("api/Forms/RoleRequest"), Authorize()> Public Function CreateRoleRequest(<FromBody> ByVal request As RoleRequest) As IHttpActionResult
            Dim principal As ClaimsPrincipal = CType(Me.User, ClaimsPrincipal)
            Dim response As IHttpActionResult = Nothing
            Dim userFactory As JestersCreditUnion.Service.Common.IUserFactory
            Dim user As User
            Dim roleRequest As New Forms.RoleRequest
            Dim formRequest As New CreateFormRequest
            Dim form As Form
            Dim formSaver As IFormSaver
            Dim tokenFactory As ITokenFactory
            Dim token As Token

            If request Is Nothing Then
                response = BadRequest("Missing or invalid request")
            End If

            If response Is Nothing Then
                Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                    userFactory = scope.Resolve(Of JestersCreditUnion.Service.Common.IUserFactory)()
                    user = userFactory.GetBySubscriberId(principal)

                    roleRequest.FullName = request.FullName
                    roleRequest.Comment = request.Comment

                    formRequest.UserId = user.UserId
                    formRequest.Content = roleRequest.Serialize.OuterXml
                    formRequest.Style = enumFormStyle.RoleRequest
                    formRequest.Type = enumFormType.RoleRequest

                    tokenFactory = scope.Resolve(Of ITokenFactory)()
                    token = tokenFactory.GetAdpToken()

                    formSaver = scope.Resolve(Of IFormSaver)()
                    form = formSaver.CreateForm(New SettingsAdp, token.AccessToken, formRequest)

                    response = Ok(form)
                End Using
            End If

            Return response
        End Function
    End Class
End Namespace