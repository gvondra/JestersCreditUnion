Imports JestersCreditUnion.Interface.UserManagement

Public Class SettingsAdp
    Implements JestersCreditUnion.Interface.UserManagement.ISettings

    Public ReadOnly Property UserManagementEndpointDomain As String Implements ISettings.EndpointDomain
        Get
            Return My.Settings.adpAuth0Domain
        End Get
    End Property
End Class
