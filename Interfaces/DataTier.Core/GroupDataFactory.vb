Public Class GroupDataFactory
    Implements IGroupDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of GroupData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of GroupData)()
    End Sub

    Public Function [Get](settings As ISettings, groupId As Guid) As GroupData Implements IGroupDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory(), groupId)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, groupId As Guid) As GroupData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "id", DbType.Guid)
        parameter.Value = groupId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "adp.sGroup",
                                             Function() New GroupData,
                                             New Action(Of IEnumerable(Of GroupData))(AddressOf AssignDataStateManager(Of GroupData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function GetAll(settings As ISettings) As IEnumerable(Of GroupData) Implements IGroupDataFactory.GetAll
        Return Me.GetAll(settings, New DbProviderFactory())
    End Function

    Public Function GetAll(settings As ISettings, ByVal providerFactory As IDbProviderFactory) As IEnumerable(Of GroupData)
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "adp.sGroupAll",
                                             Function() New GroupData,
                                             New Action(Of IEnumerable(Of GroupData))(AddressOf AssignDataStateManager(Of GroupData)))
    End Function
End Class
