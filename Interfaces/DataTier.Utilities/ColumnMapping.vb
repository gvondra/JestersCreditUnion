Imports System.Reflection
Public Class ColumnMapping
    Public Property MappingAttribute As ColumnMappingAttribute
    Public Property Info As PropertyInfo

    Public Sub SetValue(ByVal model As Object, ByVal value As Object)
        Info.SetValue(model, value)
    End Sub
End Class
