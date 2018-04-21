Imports System.Web
Public Class UserFactory
    Implements IUserFactory

    Public Function GetBySubscriberId(settings As ISettings, ByVal accessToken As String, subscriberId As String) As User Implements IUserFactory.GetBySubscriberId
        Dim uriBuilder As New UriBuilder(settings.BaseAddress)
        Dim webRequest = New WebRequest()

        uriBuilder.Path &= "user"
        uriBuilder.Query = "subscriberId=" & HttpUtility.UrlEncode(subscriberId)

        Return webRequest.Get(Of User)(uriBuilder.Uri, accessToken)
    End Function
End Class
