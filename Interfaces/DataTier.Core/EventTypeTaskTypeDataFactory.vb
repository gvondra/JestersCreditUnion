Public Class EventTypeTaskTypeDataFactory
    Implements IEventTypeTaskTypeDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of EventTypeTaskTypeData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of EventTypeTaskTypeData)()
    End Sub

    Public Function GetByTaskTypeId(settings As ISettings, taskTypeId As Guid) As IEnumerable(Of EventTypeTaskTypeData) Implements IEventTypeTaskTypeDataFactory.GetByTaskTypeId
        Return Me.GetByTaskId(settings, New DbProviderFactory(), taskTypeId)
    End Function

    Public Function GetByTaskId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, taskTypeId As Guid) As IEnumerable(Of EventTypeTaskTypeData)
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim data As EventTypeTaskTypeData
        Dim result As List(Of EventTypeTaskTypeData)
        Dim eventTypes As IEnumerable(Of EventTypeData)
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "jcu.sEventTypeTaskTypeByTaskTypeId"
                command.CommandType = CommandType.StoredProcedure
                parameter = CreateParameter(providerFactory, "taskTypeId", DbType.Guid)
                parameter.Value = taskTypeId
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader()
                result = New List(Of EventTypeTaskTypeData)(Me.GenericDataFactory.LoadData(Of EventTypeTaskTypeData)(
                    reader, Function() New EventTypeTaskTypeData,
                    New Action(Of IEnumerable(Of EventTypeTaskTypeData))(AddressOf AssignDataStateManager(Of EventTypeTaskTypeData))
                ))

                If reader.NextResult Then
                    eventTypes = Me.GenericDataFactory.LoadData(Of EventTypeData)(
                        reader,
                        Function() New EventTypeData,
                        New Action(Of IEnumerable(Of EventTypeData))(AddressOf AssignDataStateManager(Of EventTypeData))
                    )
                    For Each data In result
                        data.EventType = eventTypes.FirstOrDefault(Function(et As EventTypeData) et.EventTypeId.Equals(data.EventTypeId))
                    Next data
                End If
                reader.Close()
            End Using
        End Using
        Return result
    End Function

    Public Function GetByEventTypeId(settings As ISettings, eventTypeId As Int16) As IEnumerable(Of EventTypeTaskTypeData) Implements IEventTypeTaskTypeDataFactory.GetByEventTypeId
        Return GetByEventTypeId(settings, New DbProviderFactory(), eventTypeId)
    End Function

    Public Function GetByEventTypeId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, eventTypeId As Int16) As IEnumerable(Of EventTypeTaskTypeData)
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim data As EventTypeTaskTypeData
        Dim result As List(Of EventTypeTaskTypeData)
        Dim taskTypes As IEnumerable(Of TaskTypeData)
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "jcu.sEventTypeTaskTypeByEventTypeId"
                command.CommandType = CommandType.StoredProcedure
                parameter = CreateParameter(providerFactory, "eventTypeId", DbType.Int16)
                parameter.Value = eventTypeId
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader()
                result = New List(Of EventTypeTaskTypeData)(Me.GenericDataFactory.LoadData(Of EventTypeTaskTypeData)(
                    reader, Function() New EventTypeTaskTypeData,
                    New Action(Of IEnumerable(Of EventTypeTaskTypeData))(AddressOf AssignDataStateManager(Of EventTypeTaskTypeData))
                ))

                If reader.NextResult Then
                    taskTypes = Me.GenericDataFactory.LoadData(Of TaskTypeData)(
                        reader,
                        Function() New TaskTypeData,
                        New Action(Of IEnumerable(Of TaskTypeData))(AddressOf AssignDataStateManager(Of TaskTypeData))
                    )
                    For Each data In result
                        data.TaskType = taskTypes.FirstOrDefault(Function(et As TaskTypeData) et.TaskTypeId.Equals(data.TaskTypeId))
                    Next data
                End If
                reader.Close()
            End Using
        End Using
        Return result
    End Function
End Class
