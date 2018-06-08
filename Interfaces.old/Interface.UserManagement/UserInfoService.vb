Imports JestersCreditUnion.Interface.Common
Public Class UserInfoService
    Implements IUserInfoService

    Public Function [Get](ByVal settings As ISettings, ByVal accessToken As String) As UserInfo Implements IUserInfoService.Get
        Dim webRequest As New WebRequest
        Dim uriBuilder As New UriBuilder($"https://{settings.EndpointDomain}/UserInfo")

        Return webRequest.Get(Of UserInfo)(uriBuilder.Uri, accessToken)
    End Function
End Class
