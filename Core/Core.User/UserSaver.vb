Public Class UserSaver
    Implements IUserSaver

    Public Sub Save(settings As ISettings, user As IUser) Implements IUserSaver.Save
        Save(settings, user, Nothing)
    End Sub

    Public Sub Save(settings As ISettings, user As IUser, subscriber As String) Implements IUserSaver.Save
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), Sub(th As ITransactionHandler) InnerSave(th, user, subscriber))
    End Sub

    Private Sub InnerSave(transactionHandler As ITransactionHandler, user As IUser, subscriber As String)
        If user.UserId.Equals(Guid.Empty) Then
            user.Create(transactionHandler)
        Else
            user.Update(transactionHandler)
        End If

        If String.IsNullOrEmpty(subscriber) = False Then
            user.CreateAccount(transactionHandler, subscriber)
        End If
    End Sub
End Class
