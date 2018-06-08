Public Class EventFactory
    Implements IEventFactory

    Private m_eventTypeFactory As IEventTypeFactory

    Public Sub New(ByVal eventTypeFactory As IEventTypeFactory)
        m_eventTypeFactory = eventTypeFactory
    End Sub

    Public Function Create(ByVal settings As ISettings, form As IForm) As IEvent Implements IEventFactory.Create
        Dim eventType As enumEventType = enumEventType.NotSet
        Dim [event] As IEvent = Nothing

        Select Case form.Type
            Case enumFormType.RoleRequest
                eventType = enumEventType.RoleRequest
        End Select

        If eventType <> enumEventType.NotSet Then
            [event] = Create(settings, eventType)
        End If
        Return [event]
    End Function

    Public Function Create(ByVal settings As ISettings, ByVal eventType As enumEventType) As IEvent
        Dim [event] As [Event] = Nothing
        If eventType <> enumEventType.NotSet Then
            [event] = New [Event](m_eventTypeFactory.Get(settings, eventType))
        End If
        Return [event]
    End Function
End Class
