Public Interface IOrganizationFactory
    Function Create() As IOrganization
    Function [Get](ByVal settings As ISettings, ByVal id As Guid) As IOrganization
    Function Search(ByVal settings As ISettings, ByVal searchText As String) As IEnumerable(Of IOrganization)
End Interface
