Imports System.Security.Claims
Public Interface IUserFactory
    Function GetBySubscriberId(ByVal principal As ClaimsPrincipal) As User
End Interface
