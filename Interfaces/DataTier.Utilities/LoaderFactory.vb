Public Class LoaderFactory
    Implements ILoaderFactory

    Public Function CreateLoader() As ILoader Implements ILoaderFactory.CreateLoader
        Return New Loader() With {.Components = New List(Of ILoaderComponent)({
                                  New StringLoaderComponent(),
                                  New IntegerLoaderComponent(),
                                  New ShortLoaderComponent(),
                                  New DecimalLoaderComponent(),
                                  New DoubleLoaderComponent(),
                                  New DateLoaderComponent(),
                                  New BytesLoaderComponent(),
                                  New BooleanLoaderComponent(),
                                  New GuidLoaderComponent(),
                                  New XmlLoaderComponent()})}
    End Function
End Class
