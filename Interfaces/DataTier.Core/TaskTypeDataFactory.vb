Public Class TaskTypeDataFactory
    Implements ITaskTypeDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of TaskTypeData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of TaskTypeData)()
    End Sub

    Public Function [Get](settings As ISettings, taskTypeId As Guid) As TaskTypeData Implements ITaskTypeDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory(), taskTypeId)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, taskTypeId As Guid) As TaskTypeData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "id", DbType.Guid)
        parameter.Value = taskTypeId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sTaskType",
                                             Function() New TaskTypeData,
                                             New Action(Of IEnumerable(Of TaskTypeData))(AddressOf AssignDataStateManager(Of TaskTypeData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function GetAll(settings As ISettings) As IEnumerable(Of TaskTypeData) Implements ITaskTypeDataFactory.GetAll
        Return Me.GetAll(settings, New DbProviderFactory())
    End Function

    Public Function GetAll(settings As ISettings, ByVal providerFactory As IDbProviderFactory) As IEnumerable(Of TaskTypeData)
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sTaskTypeAll",
                                             Function() New TaskTypeData,
                                             New Action(Of IEnumerable(Of TaskTypeData))(AddressOf AssignDataStateManager(Of TaskTypeData)))
    End Function
End Class
