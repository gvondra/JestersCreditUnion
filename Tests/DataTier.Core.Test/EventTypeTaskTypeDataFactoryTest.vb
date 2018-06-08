Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class EventTypeTaskTypeDataFactoryTest

    <TestMethod()>
    Public Sub GetByTaskIdTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of EventTypeTaskTypeData))()
        Dim factory As New EventTypeTaskTypeDataFactory() With {.GenericDataFactory = generic.Object}
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

        factory.GetByTaskId(settings.Object, providerFactory.Object, Guid.Empty)

        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "jcu.sEventTypeTaskTypeByTaskTypeId", Times.Once)

        generic.Verify(Of IEnumerable(Of EventTypeTaskTypeData))(
            Function(g As IGenericDataFactory(Of EventTypeTaskTypeData)) g.LoadData(Of EventTypeTaskTypeData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of EventTypeTaskTypeData)),
                It.IsAny(Of Action(Of IEnumerable(Of EventTypeTaskTypeData)))
            ),
            Times.Once
        )

        generic.Verify(Of IEnumerable(Of EventTypeData))(
            Function(g As IGenericDataFactory(Of EventTypeTaskTypeData)) g.LoadData(Of EventTypeData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of EventTypeData)),
                It.IsAny(Of Action(Of IEnumerable(Of EventTypeData)))
            ),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetByEventTypeIdTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of EventTypeTaskTypeData))()
        Dim factory As New EventTypeTaskTypeDataFactory() With {.GenericDataFactory = generic.Object}
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

        factory.GetByEventTypeId(settings.Object, providerFactory.Object, 1S)

        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "jcu.sEventTypeTaskTypeByEventTypeId", Times.Once)

        generic.Verify(Of IEnumerable(Of EventTypeTaskTypeData))(
            Function(g As IGenericDataFactory(Of EventTypeTaskTypeData)) g.LoadData(Of EventTypeTaskTypeData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of EventTypeTaskTypeData)),
                It.IsAny(Of Action(Of IEnumerable(Of EventTypeTaskTypeData)))
            ),
            Times.Once
        )

        generic.Verify(Of IEnumerable(Of TaskTypeData))(
            Function(g As IGenericDataFactory(Of EventTypeTaskTypeData)) g.LoadData(Of TaskTypeData)(
                It.IsAny(Of IDataReader),
                It.IsAny(Of Func(Of TaskTypeData)),
                It.IsAny(Of Action(Of IEnumerable(Of TaskTypeData)))
            ),
            Times.Once
        )
    End Sub

End Class
