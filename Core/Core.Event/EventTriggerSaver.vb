Public Class EventTriggerSaver
    Implements IEventTriggerAggregator

    Private m_eventSaver As IEventSaver
    Private m_aggregator As New EventTriggerAggregator

    Public Sub New(ByVal eventSaver As IEventSaver)
        m_eventSaver = eventSaver
    End Sub

    Public Sub Add(trigger As IEventTrigger) Implements IEventTriggerAggregator.Add
        m_aggregator.Add(trigger)
    End Sub

    Public Sub Trigger(settings As ISettings, [event] As IEvent) Implements IEventTrigger.Trigger
        m_eventSaver.Create(settings, [event])

        m_aggregator.Trigger(settings, [event])

        m_eventSaver.Create(settings, [event])
    End Sub
End Class
