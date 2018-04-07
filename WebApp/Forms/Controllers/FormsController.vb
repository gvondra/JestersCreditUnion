Imports System.Net
Imports System.Web.Http

Namespace Controllers
    Public Class FormsController
        Inherits ApiController

        <HttpPost(), Authorize()> Public Function CreateRoleRequest(<FromBody> ByVal request As RoleRequest) As IHttpActionResult
            Dim response As IHttpActionResult = Nothing
            Dim roleRequest As New Forms.RoleRequest
            Dim form As New Form

            If request Is Nothing Then
                response = BadRequest("Missing or invalid request")
            End If

            If response Is Nothing Then
                roleRequest.FullName = request.FullName
                roleRequest.Comment = request.Comment

                form.Content = roleRequest.Serialize.OuterXml
                form.UserId = request.UserId
                form.StyleId = enumFormStyle.RoleRequest
                form.TypeId = enumFormType.RoleRequest

                response = Ok(form)
            End If

            Return response
        End Function
    End Class
End Namespace