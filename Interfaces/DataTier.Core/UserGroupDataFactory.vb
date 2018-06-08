Public Class UserGroupDataFactory
    Implements IUserGroupDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of UserGroupData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of UserGroupData)()
    End Sub

    Public Function GetByUserId(settings As ISettings, userId As Guid) As IEnumerable(Of UserGroupData) Implements IUserGroupDataFactory.GetByUserId
        Return Me.GetByUserId(settings, New DbProviderFactory(), userId)
    End Function

    Public Function GetByUserId(settings As ISettings, ByVal providerFactory As IDbProviderFactory, userId As Guid) As IEnumerable(Of UserGroupData)
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim data As UserGroupData
        Dim result As List(Of UserGroupData)
        Dim groups As IEnumerable(Of GroupData)
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "adp.sUserGroupByUserId"
                command.CommandType = CommandType.StoredProcedure
                parameter = CreateParameter(providerFactory, "userId", DbType.Guid)
                parameter.Value = userId
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader()
                result = New List(Of UserGroupData)(Me.GenericDataFactory.LoadData(Of UserGroupData)(
                    reader, Function() New UserGroupData,
                    New Action(Of IEnumerable(Of UserGroupData))(AddressOf AssignDataStateManager(Of UserGroupData))
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
