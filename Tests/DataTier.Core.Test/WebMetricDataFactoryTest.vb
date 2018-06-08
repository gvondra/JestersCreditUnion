Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class WebMetricDataFactoryTest

    <TestMethod()>
    Public Sub GetByMaxCreateTimestampTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of WebMetricData))()
        Dim factory As New WebMetricDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()
        Dim connection As New Mock(Of IDbConnection)()
        Dim command As New Mock(Of IDbCommand)()
        Dim parameters As New Mock(Of IDataParameterCollection)()
        Dim reader As New Mock(Of IDataReader)

        reader.Setup(Of Boolean)(Function(r As IDataReader) r.NextResult).Returns(True)
        command.Setup(Of IDataReader)(Function(c As IDbCommand) c.ExecuteReader).Returns(reader.Object)
        command.SetupGet(Of IDataParameterCollection)(Function(c As IDbCommand) c.Parameters).Returns(parameters.Object)
        connection.Setup(Of IDbCommand)(Function(c As IDbConnection) c.CreateCommand).Returns(command.Object)
        providerFactory.Setup(Of IDbConnection)(Function(p As IDbProviderFactory) p.OpenConnection(It.IsAny(Of String))).Returns(connection.Object)
        providerFactory.Setup(Of IDbDataParameter)(Function(p As IDbProviderFactory) (p.CreateParameter())).Returns(Function() New Mock(Of IDbDataParameter)().Object())

        factory.GetByMaxCreateTimestamp(settings.Object, providerFactory.Object, Date.UtcNow, 0, 25)

        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "jcu.sWebMetricByUntil", Times.Once)

        generic.Verify(Of IEnumerable(Of WebMetricData))(
            Function(g As IGenericDataFactory(Of WebMetricData)) g.LoadData(Of WebMetricData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of WebMetricData))
            ),
            Times.Once
        )

        generic.Verify(Of IEnumerable(Of WebMetricAttributeData))(
            Function(g As IGenericDataFactory(Of WebMetricData)) g.LoadData(Of WebMetricAttributeData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of WebMetricAttributeData))
            ),
            Times.Once
        )
    End Sub

End Class
