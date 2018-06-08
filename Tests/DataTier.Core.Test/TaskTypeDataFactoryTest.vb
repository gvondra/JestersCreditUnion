Imports Moq
<TestClass()>
Public Class TaskTypeDataFactoryTest

    <TestMethod()>
    Public Sub GetTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of TaskTypeData))()
        Dim factory As New TaskTypeDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.Get(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of TaskTypeData))(
            Function(f As IGenericDataFactory(Of TaskTypeData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sTaskType",
                                                                      It.IsAny(Of Func(Of TaskTypeData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of TaskTypeData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetAllTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of TaskTypeData))()
        Dim factory As New TaskTypeDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetAll(settings.Object, providerFactory.Object)

        generic.Verify(Of IEnumerable(Of TaskTypeData))(
            Function(f As IGenericDataFactory(Of TaskTypeData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sTaskTypeAll",
                                                                      It.IsAny(Of Func(Of TaskTypeData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of TaskTypeData)))),
            Times.Once
        )
    End Sub

End Class
