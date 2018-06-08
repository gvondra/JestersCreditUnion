Public Class TaskTypeGroupSaver
    Implements ITaskTypeGroupSaver

    Public Sub Save(settings As ISettings, types As IEnumerable(Of ITaskTypeGroup)) Implements ITaskTypeGroupSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler)
                                                   For Each g As ITaskTypeGroup In types
                                                       g.Save(th)
                                                   Next
                                               End Sub)
    End Sub

End Class
