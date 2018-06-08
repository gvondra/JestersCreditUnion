Imports JestersCreditUnion.DataTier.Utilities

Public Class TransactionHandler
    Inherits Settings
    Implements JestersCreditUnion.DataTier.Utilities.ITransactionHandler

    Private m_transactionHandler As Framework.ITransactionHandler

    Public Sub New(ByVal transactionHandler As Framework.ITransactionHandler)
        MyBase.New(transactionHandler)
        m_transactionHandler = transactionHandler
    End Sub

    Private Property DbConnection As IDbConnection Implements ITransactionHandler.Connection
        Get
            Return m_transactionHandler.DbConnection
        End Get
        Set(value As IDbConnection)
            m_transactionHandler.DbConnection = value
        End Set
    End Property

    Private Property DbTransaction As IDbTransaction Implements ITransactionHandler.Transaction
        Get
            Return m_transactionHandler.DbTransaction
        End Get
        Set(value As IDbTransaction)
            m_transactionHandler.DbTransaction = value
        End Set
    End Property
End Class
