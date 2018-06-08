Public Interface IOrganizationDataFactory
    Function [Get](ByVal settings As ISettings, ByVal id As Guid) As OrganizationData
    Function Search(ByVal settings As ISettings, ByVal searchText As String) As IEnumerable(Of OrganizationData)
End Interface
