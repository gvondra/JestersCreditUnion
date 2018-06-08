Public Class GroupSaver
    Implements IGroupSaver

    Public Sub Save(settings As ISettings, group As IGroup) Implements IGroupSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler) InnerSave(th, group))
    End Sub

    Private Sub InnerSave(transactionHandler As ITransactionHandler, group As IGroup)
        If group.GroupId.Equals(Guid.Empty) Then
            group.Create(transactionHandler)
        Else
            group.Update(transactionHandler)
        End If
    End Sub
End Class
