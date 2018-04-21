Public Interface ITokenService
    Function GetToken(ByVal clientId As String, ByVal secret As String, ByVal audience As String) As Token
End Interface
