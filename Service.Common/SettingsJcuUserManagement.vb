Imports JestersCreditUnion.Interface.UserManagement

Public Class SettingsJcuUserManagement
    Implements JestersCreditUnion.Interface.UserManagement.ISettings

    Public ReadOnly Property EndpointDomain As String Implements ISettings.EndpointDomain
        Get
            Return My.Settings.jcuAuth0Domain
        End Get
    End Property
End Class
