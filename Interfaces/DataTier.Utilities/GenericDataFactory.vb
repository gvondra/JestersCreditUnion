Public Class GenericDataFactory(Of T)
    Implements IGenericDataFactory(Of T)

    Public Property LoaderFactory As ILoaderFactory

    Public Sub New()
        Me.LoaderFactory = New LoaderFactory()
    End Sub

    Public Sub New(ByVal loaderFactory As ILoaderFactory)
        Me.LoaderFactory = loaderFactory
    End Sub

    Public Function GetData(settings As ISettings, providerFactory As IDbProviderFactory, procedureName As String, createModelObject As Func(Of T), assignDataStateManager As Action(Of IEnumerable(Of T))) As IEnumerable(Of T) Implements IGenericDataFactory(Of T).GetData
        Dim data As IEnumerable(Of T) = GetData(settings, providerFactory, procedureName, createModelObject)

        If assignDataStateManager IsNot Nothing Then
            assignDataStateManager.Invoke(data)
        End If

        Return data
    End Function

    Public Function GetData(settings As ISettings, providerFactory As IDbProviderFactory, procedureName As String, createModelObject As Func(Of T), assignDataStateManager As Action(Of IEnumerable(Of T)), parameters As IEnumerable(Of IDataParameter)) As IEnumerable(Of T) Implements IGenericDataFactory(Of T).GetData
        Dim data As IEnumerable(Of T) = GetData(settings, providerFactory, procedureName, createModelObject, parameters)

        If assignDataStateManager IsNot Nothing Then
            assignDataStateManager.Invoke(data)
        End If

        Return data
    End Function

    Public Function GetData(settings As ISettings, providerFactory As IDbProviderFactory, procedureName As String, createModelObject As Func(Of T)) As IEnumerable(Of T) Implements IGenericDataFactory(Of T).GetData
        Return GetData(settings, providerFactory, procedureName, createModelObject, parameters:=Nothing)
    End Function

    Public Function GetData(settings As ISettings, providerFactory As IDbProviderFactory, procedureName As String, createModelObject As Func(Of T), parameters As IEnumerable(Of IDataParameter)) As IEnumerable(Of T) Implements IGenericDataFactory(Of T).GetData
        Dim parameter As IDbDataParameter
        Dim reader As IDataReader
        Dim result As IEnumerable(Of T)

        Using connection As IDbConnection = providerFactory.OpenConnection(settings.ConnectionString)
            Using command As IDbCommand = connection.CreateCommand
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = procedureName

                If parameters IsNot Nothing Then
                    For Each parameter In parameters
                        command.Parameters.Add(parameter)
                    Next
                End If

                reader = command.ExecuteReader()
                Try
                    result = LoadData(reader, createModelObject)
                Finally
                    reader.Close()
                    reader.Dispose()
                End Try
            End Using
        End Using

        Return result
    End Function

    Public Function LoadData(Of R)(reader As IDataReader, createModelObject As Func(Of R), ByVal assignDataStateManager As Action(Of IEnumerable(Of R))) As IEnumerable(Of R) Implements IGenericDataFactory(Of T).LoadData
        Dim data As IEnumerable(Of R) = LoadData(Of R)(reader, createModelObject)

        If assignDataStateManager IsNot Nothing Then
            assignDataStateManager.Invoke(data)
        End If

        Return data
    End Function

    Public Function LoadData(Of R)(reader As IDataReader, createModelObject As Func(Of R)) As IEnumerable(Of R) Implements IGenericDataFactory(Of T).LoadData
        Return LoadData(Of R)(Me.LoaderFactory.CreateLoader, reader, createModelObject)
    End Function

    Public Function LoadData(Of R)(ByVal loader As ILoader, ByVal reader As IDataReader, ByVal createModelObject As Func(Of R)) As IEnumerable(Of R)
        Dim result As List(Of R)
        Dim data As R

        result = New List(Of R)
        While reader.Read
            data = createModelObject.Invoke()
            loader.Load(data, reader)
            result.Add(data)
        End While
        Return result
    End Function
End Class
