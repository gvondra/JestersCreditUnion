Public Class Settings
    Implements ISettings

    Public ReadOnly Property AuthEndpointDomain As String Implements ISettings.AuthEndpointDomain
        Get
            Return My.Settings.AuthEndpointDomain
        End Get
    End Property

    Public ReadOnly Property ConnectionString As String Implements ISettings.ConnectionString
        Get
            Return My.Settings.ConnectionString
        End Get
    End Property
End Class
