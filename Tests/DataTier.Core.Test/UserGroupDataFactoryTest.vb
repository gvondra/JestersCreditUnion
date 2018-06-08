Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class UserGroupDataFactoryTest

    <TestMethod()>
    Public Sub GetByUserIdTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of UserGroupData))()
        Dim factory As New UserGroupDataFactory() With {.GenericDataFactory = generic.Object}
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

        factory.GetByUserId(settings.Object, providerFactory.Object, Guid.Empty)

        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "adp.sUserGroupByUserId", Times.Once)

        generic.Verify(Of IEnumerable(Of UserGroupData))(
            Function(g As IGenericDataFactory(Of UserGroupData)) g.LoadData(Of UserGroupData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of UserGroupData)),
                It.IsAny(Of Action(Of IEnumerable(Of UserGroupData)))
            ),
            Times.Once
        )

        generic.Verify(Of IEnumerable(Of GroupData))(
            Function(g As IGenericDataFactory(Of UserGroupData)) g.LoadData(Of GroupData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of GroupData)),
                It.IsAny(Of Action(Of IEnumerable(Of GroupData)))
            ),
            Times.Once
        )
    End Sub

End Class
