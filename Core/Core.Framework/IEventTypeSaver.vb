Public Interface IEventTypeSaver
    Sub Create(ByVal settings As ISettings, ByVal eventType As IEventType)
    Sub Update(ByVal settings As ISettings, ByVal eventType As IEventType)
End Interface
