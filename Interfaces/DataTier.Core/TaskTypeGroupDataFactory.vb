Public Class TaskTypeGroupDataFactory
    Implements ITaskTypeGroupDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of TaskTypeGroupData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of TaskTypeGroupData)()
    End Sub

    Public Function GetByTaskTypeId(settings As ISettings, taskTypeId As Guid) As IEnumerable(Of TaskTypeGroupData) Implements ITaskTypeGroupDataFactory.GetByTaskTypeId
        Return Me.GetByTaskTypeId(settings, New DbProviderFactory(), taskTypeId)
    End Function

    Public Function GetByTaskTypeId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, taskTypeId As Guid) As IEnumerable(Of TaskTypeGroupData)
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim data As TaskTypeGroupData
        Dim result As List(Of TaskTypeGroupData)
        Dim groups As IEnumerable(Of GroupData)
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "adp.sTaskTypeGroupByTaskTypeId"
                command.CommandType = CommandType.StoredProcedure
                parameter = CreateParameter(providerFactory, "taskTypeId", DbType.Guid)
                parameter.Value = taskTypeId
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader()
                result = New List(Of TaskTypeGroupData)(Me.GenericDataFactory.LoadData(Of TaskTypeGroupData)(
                    reader, Function() New TaskTypeGroupData,
                    New Action(Of IEnumerable(Of TaskTypeGroupData))(AddressOf AssignDataStateManager(Of TaskTypeGroupData))
                ))

                If reader.NextResult Then
                    groups = Me.GenericDataFactory.LoadData(Of GroupData)(
                        reader,
                        Function() New GroupData,
                        New Action(Of IEnumerable(Of GroupData))(AddressOf AssignDataStateManager(Of GroupData))
                    )
                    For Each data In result
                        data.Group = groups.FirstOrDefault(Function(g As GroupData) g.GroupId.Equals(data.GroupId))
                    Next data
                End If
                reader.Close()
            End Using
        End Using
        Return result
    End Function
End Class
