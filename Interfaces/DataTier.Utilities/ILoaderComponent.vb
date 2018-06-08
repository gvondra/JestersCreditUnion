Public Interface ILoaderComponent
    Function IsApplicable(ByVal mapping As ColumnMapping) As Boolean
    Function GetValue(ByVal reader As IDataReader, ByVal ordinal As Integer) As Object
End Interface
