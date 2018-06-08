Imports System.Security.Claims
Public Interface IUserFactory
    Inherits JestersCreditUnion.Core.Framework.IUserFactory

    Overloads Function [Get](ByVal principal As ClaimsPrincipal) As IUser
End Interface
