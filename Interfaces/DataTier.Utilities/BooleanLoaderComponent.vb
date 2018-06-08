Public Class BooleanLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Return reader.GetBoolean(ordinal)
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(Boolean)) OrElse mapping.Info.PropertyType.Equals(GetType(Boolean?))
    End Function
End Class
