Imports JestersCreditUnion.Interface.UserManagement
Public Class Token
    Private Shared m_token As Token

    Public Property AccessToken As String
    Public Property Expiration As Date

    Private Sub New(ByVal innerToken As JestersCreditUnion.Interface.UserManagement.Token)
        Me.AccessToken = innerToken.access_token
        Me.Expiration = Date.Now.AddSeconds(innerToken.expires_in.Value)
    End Sub

    Public Shared Function GetToken() As Token
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
        Dim service As New TokenService(New SettingsAdp)
        Dim innerToken As JestersCreditUnion.Interface.UserManagement.Token

        innerToken = service.GetToken(My.Settings.adpClientId, My.Settings.adpSecret, My.Settings.adpAudience)
        Return New Token(innerToken)
    End Function
End Class
