Public Interface IUserInfoService
    Function [Get](ByVal settings As ISettings, ByVal accessToken As String) As UserInfo
End Interface
