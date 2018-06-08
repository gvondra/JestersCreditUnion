Public Interface ISavable
    Sub Create(ByVal transactionHandler As ITransactionHandler)
    Sub Update(ByVal transactionHandler As ITransactionHandler)
End Interface
