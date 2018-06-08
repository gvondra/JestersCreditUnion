Public Class UserGroupSaver
    Implements IUserGroupSaver

    Public Sub Save(settings As ISettings, userGroups As IEnumerable(Of IUserGroup)) Implements IUserGroupSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler) InnerSave(th, userGroups))
    End Sub

    Private Sub InnerSave(transactionHandler As ITransactionHandler, userGroups As IEnumerable(Of IUserGroup))
        Dim userGroup As IUserGroup

        For Each userGroup In userGroups
            userGroup.Save(transactionHandler)
        Next
    End Sub
End Class
