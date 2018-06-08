<TestClass()>
Public Class EventDataSaverTest

    <TestMethod()>
    Public Sub CreateTest()
        Dim transactionHandler As New Mock(Of ITransactionHandler)()
        Dim data As New EventData()
        Dim saver As New EventDataSaver(transactionHandler.Object, data)
        Dim providerFactory As New Mock(Of IDbProviderFactory)()
        Dim connection As New Mock(Of IDbConnection)()
        Dim command As New Mock(Of IDbCommand)
        Dim parameters As New Mock(Of IDataParameterCollection)

        transactionHandler.SetupGet(Of JestersCreditUnion.DataTier.Utilities.IDbTransaction)(Function(th As ITransactionHandler) th.Transaction).Returns(New Mock(Of JestersCreditUnion.DataTier.Utilities.IDbTransaction)().Object)
        providerFactory.Setup(Sub(f As IDbProviderFactory) f.EstablishTransaction(transactionHandler.Object, data)) _
        .Callback(Sub()
                      command.SetupGet(Of IDataParameterCollection)(Function(c As IDbCommand) c.Parameters).Returns(parameters.Object)
                      connection.Setup(Of IDbCommand)(Function(c As IDbConnection) c.CreateCommand).Returns(command.Object)
                      transactionHandler.SetupGet(Of IDbConnection)(Function(th As ITransactionHandler) th.Connection).Returns(connection.Object)
                  End Sub)

        providerFactory.Setup(Of IDbDataParameter)(Function(f As IDbProviderFactory) f.CreateParameter()) _
        .Returns(Function() New Mock(Of IDbDataParameter)().Object)

        saver.Create(providerFactory.Object)
        providerFactory.Verify(Sub(f As IDbProviderFactory) f.EstablishTransaction(transactionHandler.Object, data), Times.Once)
        command.Verify(Of Integer)(Function(c As IDbCommand) c.ExecuteNonQuery(), Times.Once)
        command.VerifySet(Sub(c As IDbCommand) c.CommandType = CommandType.StoredProcedure, Times.AtLeastOnce)
        command.VerifySet(Sub(c As IDbCommand) c.CommandText = "jcu.iEvent", Times.AtLeastOnce)
    End Sub

End Class
