Public Class GuidLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Return CType(reader.GetValue(ordinal), Guid)
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(Guid)) OrElse mapping.Info.PropertyType.Equals(GetType(Guid?))
    End Function
End Class
