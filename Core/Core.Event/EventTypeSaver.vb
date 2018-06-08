Public Class EventTypeSaver
    Implements IEventTypeSaver

    Public Sub Create(settings As ISettings, eventType As IEventType) Implements IEventTypeSaver.Create
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), AddressOf eventType.Create)
    End Sub

    Public Sub Update(settings As ISettings, eventType As IEventType) Implements IEventTypeSaver.Update
        Dim saver As New Saver()
        saver.Save(New CoreSettings(settings), AddressOf eventType.Update)
    End Sub

End Class
