Imports JestersCreditUnion.DataTier.Utilities

Public Class Settings
    Implements JestersCreditUnion.DataTier.Utilities.ISettings

    Private m_innerSettings As Framework.ISettings

    Public Sub New(ByVal settings As Framework.ISettings)
        m_innerSettings = settings
    End Sub

    Public ReadOnly Property ConnectionString As String Implements ISettings.ConnectionString
        Get
            Return m_innerSettings.ConnectionString
        End Get
    End Property

End Class
