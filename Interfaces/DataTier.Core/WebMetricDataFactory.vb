Public Class WebMetricDataFactory
    Implements IWebMetricDataFactory

    Public Property GenericDataFactory As IGenericDataFactory(Of WebMetricData)

    Public Sub New()
        Me.GenericDataFactory = New GenericDataFactory(Of WebMetricData)()
    End Sub

    Public Function GetByMaxCreateTimestamp(settings As ISettings, until As DateTime, offset As Int32, rowLimit As Int32) As IEnumerable(Of WebMetricData) Implements IWebMetricDataFactory.GetByMaxCreateTimestamp
        Return GetByMaxCreateTimestamp(settings, New DbProviderFactory, until, offset, rowLimit)
    End Function

    Public Function GetByMaxCreateTimestamp(settings As ISettings, ByVal providerFactory As IDbProviderFactory, until As DateTime, offset As Int32, rowLimit As Int32) As IEnumerable(Of WebMetricData)
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim data As WebMetricData
        Dim result As List(Of WebMetricData)
        Dim attributes As IEnumerable(Of WebMetricAttributeData)
        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandText = "jcu.sWebMetricByUntil"
                command.CommandType = CommandType.StoredProcedure

                parameter = CreateParameter(providerFactory, "until", DbType.DateTime)
                parameter.Value = until
                command.Parameters.Add(parameter)

                parameter = CreateParameter(providerFactory, "rowLimit", DbType.Int32)
                parameter.Value = rowLimit
                command.Parameters.Add(parameter)

                parameter = CreateParameter(providerFactory, "offset", DbType.Int32)
                parameter.Value = offset
                command.Parameters.Add(parameter)

                reader = command.ExecuteReader()
                result = New List(Of WebMetricData)(
                    Me.GenericDataFactory.LoadData(Of WebMetricData)(
                        reader, Function() New WebMetricData
                    ) _
                    .Select(Function(wm As WebMetricData)
                                wm.CreateTimestamp = Date.SpecifyKind(wm.CreateTimestamp, DateTimeKind.Utc)
                                Return wm
                            End Function)
                )

                If reader.NextResult Then
                    attributes = Me.GenericDataFactory.LoadData(Of WebMetricAttributeData)(
                        reader, Function() New WebMetricAttributeData
                    )
                    For Each data In result
                        data.Attributes = attributes.Where(Function(a As WebMetricAttributeData) a.WebMetricId.Equals(data.WebMetricId))
                    Next data
                End If
                reader.Close()
            End Using
        End Using
        Return result
    End Function
End Class
