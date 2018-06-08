Public Class TaskTypeSaver
    Implements ITaskTypeSaver

    Public Sub Save(settings As ISettings, taskType As ITaskType) Implements ITaskTypeSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler) InnerSaver(th, taskType))
    End Sub

    Private Sub InnerSaver(transactionHandler As ITransactionHandler, taskType As ITaskType)
        If taskType.TaskTypeId.Equals(Guid.Empty) Then
            taskType.Create(transactionHandler)
        Else
            taskType.Update(transactionHandler)
        End If
    End Sub
End Class
