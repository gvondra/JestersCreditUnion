Imports Moq
<TestClass()>
Public Class GroupDataFactoryTest

    <TestMethod()>
    Public Sub GetTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of GroupData))()
        Dim factory As New GroupDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.Get(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of GroupData))(
            Function(f As IGenericDataFactory(Of GroupData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sGroup",
                                                                      It.IsAny(Of Func(Of GroupData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of GroupData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetAllTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of GroupData))()
        Dim factory As New GroupDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetAll(settings.Object, providerFactory.Object)

        generic.Verify(Of IEnumerable(Of GroupData))(
            Function(f As IGenericDataFactory(Of GroupData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sGroupAll",
                                                                      It.IsAny(Of Func(Of GroupData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of GroupData)))),
            Times.Once
        )
    End Sub

End Class
