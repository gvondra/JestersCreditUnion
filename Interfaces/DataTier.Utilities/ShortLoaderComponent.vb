Public Class ShortLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Return reader.GetInt16(ordinal)
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(Short)) OrElse mapping.Info.PropertyType.Equals(GetType(Short?))
    End Function
End Class
