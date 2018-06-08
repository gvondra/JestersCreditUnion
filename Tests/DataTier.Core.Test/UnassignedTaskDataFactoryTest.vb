Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class UnassignedTaskDataFactoryTest

    <TestMethod()>
    Public Sub GetByMaxCreateTimestampTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UnassignedTaskData))()
        Dim factory As New UnassignedTaskDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetByUser(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of UnassignedTaskData))(
            Function(f As IGenericDataFactory(Of UnassignedTaskData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "jcu.sUnassignedTaskByUser",
                                                                      It.IsAny(Of Func(Of UnassignedTaskData)),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

End Class
