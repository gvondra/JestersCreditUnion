Public Class EventTriggerAggregator
    Implements IEventTriggerAggregator

    Private m_events As New List(Of IEventTrigger)

    Public Sub Add(trigger As IEventTrigger) Implements IEventTriggerAggregator.Add
        m_events.Add(trigger)
    End Sub

    Public Sub Trigger(settings As ISettings, [event] As IEvent) Implements IEventTrigger.Trigger
        Dim tasks As IEnumerable(Of Task)

        tasks = m_events.Select(Of Task)(Function(t As IEventTrigger) Task.Run(Sub() t.Trigger(New CoreSettings(settings), [event])))
        Task.WaitAll(tasks.ToArray)
    End Sub
End Class
