Imports Moq
<TestClass()>
Public Class UserDataFactoryTest

    <TestMethod()>
    Public Sub GetByEmailAddressTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UserData))()
        Dim factory As New UserDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetByEmailAddress(settings.Object, providerFactory.Object, "test email address")

        generic.Verify(Of IEnumerable(Of UserData))(
            Function(f As IGenericDataFactory(Of UserData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sUserByEmailAddress",
                                                                      It.IsAny(Of Func(Of UserData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of UserData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetBySubscriberIdTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UserData))()
        Dim factory As New UserDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetBySubscriberId(settings.Object, providerFactory.Object, "test subscriber")

        generic.Verify(Of IEnumerable(Of UserData))(
            Function(f As IGenericDataFactory(Of UserData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sUserBySubscriberId",
                                                                      It.IsAny(Of Func(Of UserData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of UserData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UserData))()
        Dim factory As New UserDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.Get(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of UserData))(
            Function(f As IGenericDataFactory(Of UserData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sUser",
                                                                      It.IsAny(Of Func(Of UserData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of UserData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub SearchTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UserData))()
        Dim factory As New UserDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.Search(settings.Object, providerFactory.Object, "test search")

        generic.Verify(Of IEnumerable(Of UserData))(
            Function(f As IGenericDataFactory(Of UserData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sUserSearch",
                                                                      It.IsAny(Of Func(Of UserData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of UserData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub
End Class
