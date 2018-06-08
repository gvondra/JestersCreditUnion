Public Interface IOrganization
    Inherits ISavable

    ReadOnly Property OrganizationId As Guid
    Property Name As String

    Sub Save(ByVal transactionHandler As ITransactionHandler)
End Interface
