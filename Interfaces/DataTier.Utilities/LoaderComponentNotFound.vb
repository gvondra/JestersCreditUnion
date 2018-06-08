Public Class LoaderComponentNotFound
    Inherits ApplicationException

    Public Sub New(ByVal columnMapping As ColumnMapping)
        MyBase.New(String.Format("Loader not found for property {0} on {1}", columnMapping.Info.Name, columnMapping.Info.DeclaringType.FullName))
    End Sub
End Class
