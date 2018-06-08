Public Class UnassignedTaskDataFactory
    Implements IUnassignedTaskDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of UnassignedTaskData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of UnassignedTaskData)()
    End Sub

    Public Function GetByUser(settings As ISettings, userId As Guid) As IEnumerable(Of UnassignedTaskData) Implements IUnassignedTaskDataFactory.GetByUser
        Return GetByUser(settings, New DbProviderFactory(), userId)
    End Function

    Public Function GetByUser(settings As ISettings, ByVal providerFactory As IDbProviderFactory, userId As Guid) As IEnumerable(Of UnassignedTaskData)
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "userId", DbType.Guid)
        parameter.Value = userId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sUnassignedTaskByUser",
                                             Function() New UnassignedTaskData,
                                             {parameter})
    End Function
End Class
