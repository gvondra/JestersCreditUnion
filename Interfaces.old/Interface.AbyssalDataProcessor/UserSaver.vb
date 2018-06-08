Imports System.IO
Public Class UserSaver
    Implements IUserSaver

    Public Function Create(settings As ISettings, ByVal accessToken As String, user As User, ByVal subscriberId As String) As User Implements IUserSaver.Create
        Dim webRequest As New WebRequest
        Dim uriBuilder As New UriBuilder(settings.BaseAddress)

        uriBuilder.Path &= "Users"

        Return webRequest.Post(Of User)(uriBuilder.Uri, accessToken,
            Sub(s As Stream) webRequest.Serialize(s, New With {.SubscriberId = subscriberId, .User = user})
        )
    End Function
End Class
