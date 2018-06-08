Public Class CoreSettings
    Implements ITransactionHandler

    Private m_innerSettings As ISettings

    Public Sub New(ByVal settings As ISettings)
        m_innerSettings = settings
    End Sub

    Public ReadOnly Property AuthEndpointDomain As String Implements ISettings.AuthEndpointDomain
        Get
            Return m_innerSettings.AuthEndpointDomain
        End Get
    End Property

    Public ReadOnly Property ConnectionString As String Implements ISettings.ConnectionString
        Get
            Return m_innerSettings.ConnectionString
        End Get
    End Property

    Public Property DbConnection As IDbConnection Implements ITransactionHandler.DbConnection
    Public Property DbTransaction As JestersCreditUnion.DataTier.Utilities.IDbTransaction Implements ITransactionHandler.DbTransaction
End Class
