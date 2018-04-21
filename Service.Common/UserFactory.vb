Imports System.Security.Claims
Public Class UserFactory
    Implements IUserFactory

    Private m_tokenFactory As ITokenFactory
    Private m_innerUserFactory As JestersCreditUnion.Interface.AbyssalDataProcessor.IUserFactory
    Private m_userInfoService As JestersCreditUnion.Interface.UserManagement.IUserInfoService
    Private m_userSaver As IUserSaver

    Public Sub New(ByVal tokenFactory As ITokenFactory,
            ByVal userFactory As JestersCreditUnion.Interface.AbyssalDataProcessor.IUserFactory,
            userInfoService As JestersCreditUnion.Interface.UserManagement.IUserInfoService,
            userSaver As IUserSaver)

        m_tokenFactory = tokenFactory
        m_innerUserFactory = userFactory
        m_userInfoService = userInfoService
        m_userSaver = userSaver
    End Sub

    Public Function GetBySubscriberId(ByVal principal As ClaimsPrincipal) As User Implements IUserFactory.GetBySubscriberId
        Dim id As Claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.NameIdentifier)
        Dim subscriberId As String = id.Value
        Dim token As Token = m_tokenFactory.GetAdpToken
        Dim user As User = m_innerUserFactory.GetBySubscriberId(New SettingsAdp, token.AccessToken, subscriberId)

        If user Is Nothing Then
            user = New User
            UpdateUser(user, principal)
            user = m_userSaver.Create(New SettingsAdp, token.AccessToken, user, subscriberId)
        End If

        Return user
    End Function

    Private Sub UpdateUser(ByVal user As User, ByVal principal As ClaimsPrincipal)
        Dim claim As Claim = Nothing
        Dim accessToken = CType(CType(principal.Identity, ClaimsIdentity).BootstrapContext, System.IdentityModel.Tokens.BootstrapContext).Token
        Dim userInfo As JestersCreditUnion.Interface.UserManagement.UserInfo

        claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.Name)
        If claim IsNot Nothing AndAlso String.IsNullOrEmpty(claim.Value) = False Then
            If String.IsNullOrEmpty(user.FullName) Then
                user.FullName = claim.Value
            End If
            If String.IsNullOrEmpty(user.ShortName) Then
                user.ShortName = claim.Value
            End If
        End If

        If principal.Claims.Where(Function(c As Claim) c.Type = "gty" AndAlso c.Value = "client-credentials").Any() Then
            claim = principal.Claims.FirstOrDefault(Function(c As Claim) c.Type = ClaimTypes.NameIdentifier)
            If claim IsNot Nothing AndAlso String.IsNullOrEmpty(user.FullName) Then
                user.FullName = claim.Value
            End If
        Else
            userInfo = m_userInfoService.Get(New SettingsJcuUserManagement, accessToken)

            If userInfo IsNot Nothing Then
                If String.IsNullOrEmpty(userInfo.name) = False Then
                    user.FullName = userInfo.name
                End If
                If String.IsNullOrEmpty(userInfo.given_name) = False Then
                    user.ShortName = userInfo.given_name
                End If
                If String.IsNullOrEmpty(userInfo.email) = False Then
                    user.EmailAddress = userInfo.email
                End If
            End If
        End If
    End Sub
End Class
