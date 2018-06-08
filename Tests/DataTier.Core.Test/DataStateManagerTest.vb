<TestClass()>
Public Class DataStateManagerTest

    <TestMethod()>
    Public Sub TestMethod1()
        Dim model As New ModelTest() With {
            .TestString = "Test String",
            .TestInteger = 1,
            .TestShort = 2S,
            .TestDecimal = 3.4D,
            .TestDouble = 5.6,
            .TestDate = #12/31/2018#,
            .TestBytes = {7, 8},
            .TestBoolan = True,
            .TestGuid = Guid.NewGuid
        }

        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.[New], model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Unchaged, model.DataStateManager.GetState(model))

        model.TestString = "New Test String"
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestInteger = 2
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestShort = 3S
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestDecimal = 3.5D
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestDouble = 5.7
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestDate = #1/1/2019#
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestBytes = {8, 9}
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestBoolan = False
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))

        model.DataStateManager.Original = CType(model.Clone(), ModelTest)
        model.TestGuid = Guid.NewGuid
        Assert.AreEqual(IDataStateManager(Of ModelTest).enumState.Updated, model.DataStateManager.GetState(model))
    End Sub

    Public Class ModelTest
        Implements IDataManagedState(Of ModelTest)

        Public Property DataStateManager As IDataStateManager(Of ModelTest) = New DataStateManager(Of ModelTest) Implements IDataManagedState(Of ModelTest).DataStateManager

        <ColumnMapping("")> Public Property TestString As String
        <ColumnMapping("")> Public Property TestInteger As Integer?
        <ColumnMapping("")> Public Property TestShort As Short?
        <ColumnMapping("")> Public Property TestDecimal As Decimal?
        <ColumnMapping("")> Public Property TestDouble As Double?
        <ColumnMapping("")> Public Property TestDate As Date?
        <ColumnMapping("")> Public Property TestBytes As Byte()
        <ColumnMapping("")> Public Property TestBoolan As Boolean?
        <ColumnMapping("")> Public Property TestGuid As Guid?

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

        Public Sub AcceptChanges() Implements IDataManagedState(Of ModelTest).AcceptChanges, IDbTransactionObserver.AfterCommit
            If DataStateManager IsNot Nothing Then DataStateManager.Original = Me
        End Sub
    End Class
End Class
