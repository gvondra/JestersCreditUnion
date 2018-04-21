Imports JestersCreditUnion.Interface.UserManagement
Public Class TokenFactory
    Implements ITokenFactory

    Private Shared m_token As Token

    Public Function GetAdpToken() As Token Implements ITokenFactory.GetAdpToken
        If m_token Is Nothing OrElse m_token.Expiration <= Date.Now Then
            SyncLock GetType(Token)
                If m_token Is Nothing OrElse m_token.Expiration <= Date.Now Then
                    m_token = GetNewToken()
                End If
            End SyncLock
        End If
        Return m_token
    End Function

    Private Shared Function GetNewToken() As Token
        Dim service As New TokenService(New SettingsAdpUserManagement)
        Dim innerToken As JestersCreditUnion.Interface.UserManagement.Token

        innerToken = service.GetToken(My.Settings.adpClientId, My.Settings.adpSecret, My.Settings.adpAudience)
        Return New Token() With {.AccessToken = innerToken.access_token, .Expiration = Date.Now.AddSeconds(innerToken.expires_in.Value)}
    End Function
End Class
