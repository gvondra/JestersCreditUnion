Public Class EventSaver
    Implements IEventSaver

    Public Sub Create(settings As ISettings, [event] As IEvent) Implements IEventSaver.Create
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), AddressOf [event].Create)
    End Sub

End Class
