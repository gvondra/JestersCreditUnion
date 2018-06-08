Public Class Saver
    Public Sub Save(ByVal transactionHandler As Framework.ITransactionHandler, ByVal save As Action(Of ITransactionHandler))
        Try
            save.Invoke(transactionHandler)

            If transactionHandler.DbTransaction IsNot Nothing Then
                transactionHandler.DbTransaction.Commit()
            End If
        Catch
            If transactionHandler.DbTransaction IsNot Nothing Then
                transactionHandler.DbTransaction.Rollback()
            End If
            Throw
        Finally
            If transactionHandler.DbTransaction IsNot Nothing Then
                transactionHandler.DbTransaction.Dispose()
                transactionHandler.DbTransaction = Nothing
            End If
            If transactionHandler.DbConnection IsNot Nothing Then
                transactionHandler.DbConnection.Dispose()
                transactionHandler.DbConnection = Nothing
            End If
        End Try
    End Sub
End Class
