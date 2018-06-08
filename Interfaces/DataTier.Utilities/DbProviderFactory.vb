Imports System.Data.Common
Public Class DbProviderFactory
    Implements IDbProviderFactory

    Private m_innerFactory As System.Data.Common.DbProviderFactory

    Public Sub New()
        m_innerFactory = DbProviderFactories.GetFactory("System.Data.SqlClient")
    End Sub

    Public Sub EstablishTransaction(transactionHandler As ITransactionHandler) Implements IDbProviderFactory.EstablishTransaction
        EstablishTransaction(transactionHandler, Nothing)
    End Sub

    Public Sub EstablishTransaction(transactionHandler As ITransactionHandler, ByVal observer As IDbTransactionObserver) Implements IDbProviderFactory.EstablishTransaction
        If transactionHandler.Connection IsNot Nothing Then
            If transactionHandler.Connection.State <> ConnectionState.Open Then
                transactionHandler.Connection.Dispose()
                transactionHandler.Connection = Nothing
            End If
        End If
        If transactionHandler.Connection Is Nothing Then
            transactionHandler.Connection = OpenConnection(transactionHandler.ConnectionString)
        End If
        If transactionHandler.Transaction Is Nothing Then
            transactionHandler.Transaction = New DbTransaction(transactionHandler.Connection.BeginTransaction)
        End If
        If transactionHandler.Transaction IsNot Nothing AndAlso observer IsNot Nothing Then
            transactionHandler.Transaction.AddObserver(observer)
        End If
    End Sub

    Public Function CreateConnection() As IDbConnection Implements IDbProviderFactory.CreateConnection
        Return m_innerFactory.CreateConnection
    End Function

    Public Function CreateParameter() As IDbDataParameter Implements IDbProviderFactory.CreateParameter
        Return m_innerFactory.CreateParameter
    End Function

    Public Function OpenConnection(connectionString As String) As IDbConnection Implements IDbProviderFactory.OpenConnection
        Dim connection As DbConnection
        If String.IsNullOrEmpty(connectionString) = False Then
            connection = m_innerFactory.CreateConnection
            connection.ConnectionString = connectionString
            connection.Open()
        Else
            connection = Nothing
        End If
        Return connection
    End Function
End Class
