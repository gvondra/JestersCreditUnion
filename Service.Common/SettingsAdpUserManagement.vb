Imports JestersCreditUnion.Interface.UserManagement

Public Class SettingsAdpUserManagement
    Implements JestersCreditUnion.Interface.UserManagement.ISettings

    Public ReadOnly Property EndpointDomain As String Implements ISettings.EndpointDomain
        Get
            Return My.Settings.adpAuth0Domain
        End Get
    End Property
End Class
