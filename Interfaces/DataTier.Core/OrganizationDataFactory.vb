Imports System.Text.RegularExpressions
Public Class OrganizationDataFactory
    Implements IOrganizationDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of OrganizationData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of OrganizationData)()
    End Sub

    Public Function [Get](settings As ISettings, id As Guid) As OrganizationData Implements IOrganizationDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory, id)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, id As Guid) As OrganizationData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "id", DbType.Guid)
        parameter.Value = id
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "adp.sOrganization",
                                             Function() New OrganizationData,
                                             New Action(Of IEnumerable(Of OrganizationData))(AddressOf AssignDataStateManager(Of OrganizationData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function Search(settings As ISettings, searchText As String) As IEnumerable(Of OrganizationData) Implements IOrganizationDataFactory.Search
        Return Me.Search(settings, New DbProviderFactory, searchText)
    End Function

    Public Function Search(settings As ISettings, ByVal providerFactory As IDbProviderFactory, searchText As String) As IEnumerable(Of OrganizationData)
        searchText = searchText.Trim
        Dim wildCardValue As IDbDataParameter = CreateParameter(providerFactory, "wildCardValue", DbType.String)
        searchText = Regex.Replace(searchText, "\\", "\\")
        searchText = Regex.Replace(searchText, "_", "\_")
        searchText = Regex.Replace(searchText, "%", "\%")
        searchText = Regex.Replace(searchText, "\s+", "%")
        wildCardValue.Value = "%" & searchText & "%"
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "adp.sOrganizationSearch",
                                             Function() New OrganizationData,
                                             New Action(Of IEnumerable(Of OrganizationData))(AddressOf AssignDataStateManager(Of OrganizationData)),
                                             {wildCardValue})
    End Function
End Class
