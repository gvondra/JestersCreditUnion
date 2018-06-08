Public Class TaskTypeEventTypeSaver
    Implements ITaskTypeEventTypeSaver

    Public Sub Save(settings As ISettings, types As IEnumerable(Of ITaskTypeEventType)) Implements ITaskTypeEventTypeSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler)
                                                   For Each t As ITaskTypeEventType In types
                                                       t.Save(th)
                                                   Next
                                               End Sub)
    End Sub
End Class
