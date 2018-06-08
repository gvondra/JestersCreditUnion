Public Interface IEventFactory
    Function Create(ByVal settings As ISettings, ByVal form As IForm) As IEvent
End Interface
