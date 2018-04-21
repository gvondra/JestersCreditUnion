Imports Autofac
Imports System.Net
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    Public Class FormsController
        Inherits ControllerBase

        <HttpPost(), Authorize()> Public Function CreateRoleRequest(<FromBody> ByVal request As RoleRequest) As IHttpActionResult
            Dim principal As ClaimsPrincipal = CType(Me.User, ClaimsPrincipal)
            Dim response As IHttpActionResult = Nothing
            Dim userFactory As JestersCreditUnion.Service.Common.IUserFactory
            Dim user As User
            'Dim roleRequest As New Forms.RoleRequest
            'Dim form As New Form

            If request Is Nothing Then
                response = BadRequest("Missing or invalid request")
            End If

            If response Is Nothing Then
                Using scope As ILifetimeScope = Me.ObjectContainer.BeginLifetimeScope
                    userFactory = scope.Resolve(Of JestersCreditUnion.Service.Common.IUserFactory)()
                    user = userFactory.GetBySubscriberId(principal)
                End Using
                '    roleRequest.FullName = request.FullName
                '    roleRequest.Comment = request.Comment

                '    form.Content = roleRequest.Serialize.OuterXml
                '    form.UserId = request.UserId
                '    form.StyleId = enumFormStyle.RoleRequest
                '    form.TypeId = enumFormType.RoleRequest

                '    response = Ok(form)
            End If

            Return response
        End Function
    End Class
End Namespace