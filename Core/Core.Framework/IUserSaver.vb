Public Interface IUserSaver
    Sub Save(ByVal settings As ISettings, ByVal user As IUser)
    Sub Save(ByVal settings As ISettings, ByVal user As IUser, ByVal subscriber As String)
End Interface
