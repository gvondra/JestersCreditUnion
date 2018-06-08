Public Class TaskSaver
    Implements ITaskSaver

    Public Sub Update(settings As ISettings, task As ITask) Implements ITaskSaver.Update
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), AddressOf task.Update)
    End Sub
End Class
