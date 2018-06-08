Public Class TaskDataFactory
    Implements ITaskDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of TaskData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of TaskData)()
    End Sub

    Public Function [Get](settings As ISettings, taskId As Guid) As TaskData Implements ITaskDataFactory.Get
        Return Me.Get(settings, New DbProviderFactory(), taskId)
    End Function

    Public Function [Get](settings As ISettings, ByVal providerFactory As IDbProviderFactory, taskId As Guid) As TaskData
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "taskId", DbType.Guid)
        parameter.Value = taskId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sTask",
                                             Function() New TaskData,
                                             New Action(Of IEnumerable(Of TaskData))(AddressOf AssignDataStateManager(Of TaskData)),
                                             {parameter}).FirstOrDefault
    End Function

    Public Function GetByUserId(settings As ISettings, userId As Guid) As IEnumerable(Of TaskData) Implements ITaskDataFactory.GetByUserId
        Return GetByUserId(settings, New DbProviderFactory, userId)
    End Function

    Public Function GetByUserId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, userId As Guid) As IEnumerable(Of TaskData)
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "userId", DbType.Guid)
        parameter.Value = userId
        Return Me.GenericDataFactory.GetData(settings,
                                             providerFactory,
                                             "jcu.sTaskByUserId",
                                             Function() New TaskData,
                                             New Action(Of IEnumerable(Of TaskData))(AddressOf AssignDataStateManager(Of TaskData)),
                                             {parameter})
    End Function

    Public Function GetFormIds(settings As ISettings, taskId As Guid) As IEnumerable(Of Guid) Implements ITaskDataFactory.GetFormIds
        Return GetFormIds(settings, New DbProviderFactory(), taskId)
    End Function

    Public Function GetFormIds(settings As ISettings, ByVal providerFactory As IDbProviderFactory, taskId As Guid) As IEnumerable(Of Guid)
        Dim parameter As IDbDataParameter = CreateParameter(providerFactory, "taskId", DbType.Guid)
        Dim reader As IDataReader
        Dim result As New List(Of Guid)
        parameter.Value = taskId
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "jcu.sTaskFormByTaskId"
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader
                While reader.Read
                    result.Add(reader.GetGuid(reader.GetOrdinal("FormId")))
                End While
                reader.Close()
            End Using
        End Using
        Return result
    End Function
End Class
