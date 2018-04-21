Imports System.Net
Imports System.Security.Claims
Imports System.Web.Http

Namespace Controllers
    Public Class UserController
        Inherits ApiController

        <HttpGet(), Authorize()> Public Function GetCurrent() As IHttpActionResult
            Dim principal As ClaimsPrincipal = CType(Me.User, ClaimsPrincipal)
            Dim token As Token = Token.GetToken()
            Return Ok()
        End Function
    End Class
End Namespace