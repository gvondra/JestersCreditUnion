Public Interface IUserSaver
    Function Create(ByVal settings As ISettings, ByVal accessToken As String, ByVal user As User, ByVal subscriberId As String) As User
End Interface
