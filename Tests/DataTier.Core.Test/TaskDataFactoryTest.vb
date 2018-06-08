<TestClass()>
Public Class TaskDataFactoryTest

    <TestMethod()>
    Public Sub GetTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of TaskData))()
        Dim factory As New TaskDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.Get(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of TaskData))(
            Function(f As IGenericDataFactory(Of TaskData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sTask",
                                                                      It.IsAny(Of Func(Of TaskData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of TaskData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetByUserIdTest()
        Dim generic As New Mock(Of IGenericDataFactory(Of TaskData))()
        Dim factory As New TaskDataFactory() With {.GenericDataFactory = generic.Object}
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()).Returns(Function() New Mock(Of IDbDataParameter)().Object)

        factory.GetByUserId(settings.Object, providerFactory.Object, Guid.Empty)

        generic.Verify(Of IEnumerable(Of TaskData))(
            Function(f As IGenericDataFactory(Of TaskData)) f.GetData(settings.Object,
                                                                      providerFactory.Object,
                                                                      "adp.sTaskByUserId",
                                                                      It.IsAny(Of Func(Of TaskData)),
                                                                      It.IsAny(Of Action(Of IEnumerable(Of TaskData))),
                                                                      It.IsAny(Of IEnumerable(Of IDataParameter))),
            Times.Once
        )
    End Sub

    <TestMethod()>
    Public Sub GetFormIdsTest()
        Dim factory As New TaskDataFactory()
        Dim settings As New Mock(Of ISettings)()
        Dim providerFactory As New Mock(Of IDbProviderFactory)()
        Dim result As IEnumerable(Of Guid)
        Dim connection As New Mock(Of IDbConnection)
        Dim command As New Mock(Of IDbCommand)
        Dim data As New DataTable

        data.Columns.Add("FormId", GetType(Guid))
        data.Rows.Add(Guid.NewGuid)
        data.Rows.Add(Guid.NewGuid)
        data.Rows.Add(Guid.NewGuid)

        providerFactory.Setup(Of IDbConnection)(Function(f As IDbProviderFactory) f.OpenConnection(It.IsAny(Of String))) _
        .Returns(
            Function()
                connection.Setup(Of IDbCommand)(Function(c As IDbConnection) c.CreateCommand) _
                .Returns(
                    Function()
                        command.Setup(Of IDataReader)(Function(c As IDbCommand) c.ExecuteReader()) _
                        .Returns(Function() data.CreateDataReader)

                        command.SetupGet(Of IDataParameterCollection)(Function(c As IDbCommand) c.Parameters) _
                        .Returns(Function() New Mock(Of IDataParameterCollection)().Object)

                        Return command.Object
                    End Function
                )
                Return connection.Object()
            End Function
        )

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter) _
        .Returns(
            Function()
                Dim p As New Mock(Of IDbDataParameter)()
                p.SetupAllProperties()
                Return p.Object
            End Function
        )

        result = factory.GetFormIds(settings.Object, providerFactory.Object, Guid.Empty)

        Assert.IsNotNull(result)
        Assert.AreEqual(data.Rows.Count, result.Count())

        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "adp.sTaskFormByTaskId", Times.Once)
    End Sub
End Class
