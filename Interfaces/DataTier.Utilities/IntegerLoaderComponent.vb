Public Class IntegerLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Return reader.GetInt32(ordinal)
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(Integer)) OrElse mapping.Info.PropertyType.Equals(GetType(Integer?))
    End Function
End Class
