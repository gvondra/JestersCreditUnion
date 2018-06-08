Public Interface IUserFactory
    Function GetBySubscriberId(ByVal settings As ISettings, ByVal accessToken As String, ByVal subscriberId As String) As User
End Interface
