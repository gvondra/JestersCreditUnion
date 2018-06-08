Public Class ColumnMappingAttribute
    Inherits Attribute

    Public Property ColumnName As String
    Public Property IsNullable As Boolean

    Public Sub New(ByVal columnName As String)
        Me.ColumnName = columnName
    End Sub
End Class
