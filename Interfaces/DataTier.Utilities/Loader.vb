Imports System.Reflection
Imports System.Threading
Public Class Loader
    Implements ILoader

    Private Shared m_columnMappings As Dictionary(Of Type, List(Of ColumnMapping))
    Private Shared m_mappingsLock As AutoResetEvent
    Public Property Components As IEnumerable(Of ILoaderComponent)

    Shared Sub New()
        m_mappingsLock = New AutoResetEvent(True)
        m_columnMappings = New Dictionary(Of Type, List(Of ColumnMapping))
    End Sub

    Public Sub Load(data As Object, reader As IDataReader) Implements ILoader.Load
        Dim columnMappings As List(Of ColumnMapping) = GetColumnMappings(data)
        Dim columnMapping As ColumnMapping
        Dim ordinal As Integer

        For Each columnMapping In columnMappings
            ordinal = reader.GetOrdinal(columnMapping.MappingAttribute.ColumnName)
            If ordinal >= 0 Then
                If columnMapping.MappingAttribute.IsNullable AndAlso reader.IsDBNull(ordinal) Then
                    columnMapping.SetValue(data, Nothing)
                Else
                    columnMapping.SetValue(data, GetValue(reader, ordinal, columnMapping))
                End If
            End If
        Next
    End Sub

    Private Function GetValue(ByVal reader As IDataReader, ByVal ordinal As Integer, ByVal columnMapping As ColumnMapping) As Object
        Dim value As Object = Nothing
        Dim component As ILoaderComponent

        If Components IsNot Nothing Then
            component = Components.Where(Function(c As ILoaderComponent) c.IsApplicable(columnMapping)).FirstOrDefault
            If component IsNot Nothing Then
                value = component.GetValue(reader, ordinal)
            Else
                Throw New LoaderComponentNotFound(columnMapping)
            End If
        End If

        Return value
    End Function

    Private Function GetColumnMappings(ByVal data As Object) As List(Of ColumnMapping)
        Dim type As Type = data.GetType

        If m_columnMappings.ContainsKey(type) = False Then
            m_mappingsLock.WaitOne()
            Try
                If m_columnMappings.ContainsKey(type) = False Then
                    m_columnMappings.Add(type, LoadColumnMappings(type))
                End If
            Finally
                m_mappingsLock.Set()
            End Try
        End If
        Return m_columnMappings(type)
    End Function

    Private Function LoadColumnMappings(ByVal type As Type) As List(Of ColumnMapping)
        Dim mappings As New List(Of ColumnMapping)
        Dim attribute As ColumnMappingAttribute
        Dim mapping As ColumnMapping
        Dim properties As IEnumerable(Of PropertyInfo) = From p In type.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
                                                         Where p.CanWrite = True

        If properties IsNot Nothing Then
            For Each p As PropertyInfo In properties
                For Each attribute In p.GetCustomAttributes(Of ColumnMappingAttribute)(True)
                    mapping = New ColumnMapping() With {.MappingAttribute = attribute, .Info = p}
                    mappings.Add(mapping)
                Next attribute
            Next p
        End If

        Return mappings
    End Function
End Class
