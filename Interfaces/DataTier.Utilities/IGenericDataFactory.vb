Public Interface IGenericDataFactory(Of T)
    Function GetData(ByVal settings As ISettings,
                     ByVal providerFactory As IDbProviderFactory,
                     ByVal procedureName As String,
                     ByVal createModelObject As Func(Of T)) As IEnumerable(Of T)
    Function GetData(ByVal settings As ISettings,
                     ByVal providerFactory As IDbProviderFactory,
                     ByVal procedureName As String,
                     ByVal createModelObject As Func(Of T),
                     ByVal parameters As IEnumerable(Of IDataParameter)) As IEnumerable(Of T)
    Function GetData(ByVal settings As ISettings,
                     ByVal providerFactory As IDbProviderFactory,
                     ByVal procedureName As String,
                     ByVal createModelObject As Func(Of T),
                     ByVal assignDataStateManager As Action(Of IEnumerable(Of T))) As IEnumerable(Of T)
    Function GetData(ByVal settings As ISettings,
                     ByVal providerFactory As IDbProviderFactory,
                     ByVal procedureName As String,
                     ByVal createModelObject As Func(Of T),
                     ByVal assignDataStateManager As Action(Of IEnumerable(Of T)),
                     ByVal parameters As IEnumerable(Of IDataParameter)) As IEnumerable(Of T)
    Function LoadData(Of R)(ByVal reader As IDataReader, ByVal createModelObject As Func(Of R)) As IEnumerable(Of R)
    Function LoadData(Of R)(ByVal reader As IDataReader, ByVal createModelObject As Func(Of R), ByVal assignDataStateManager As Action(Of IEnumerable(Of R))) As IEnumerable(Of R)
End Interface
