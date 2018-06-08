Public Class DecimalLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Return reader.GetDecimal(ordinal)
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(Decimal)) OrElse mapping.Info.PropertyType.Equals(GetType(Decimal?))
    End Function
End Class
