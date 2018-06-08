Public Class FormSaver
    Implements IFormSaver

    Public Sub Create(settings As ISettings, form As IForm) Implements IFormSaver.Create
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), AddressOf form.Create)
    End Sub
End Class
