Public Interface IUserUpdater
    Sub UpdateFromUserManagement(ByVal settings As ISettings, ByVal user As IUser, ByVal accessToken As String)
End Interface
