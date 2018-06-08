Public Class EventTriggerFactory
    Implements IEventTriggerFactory

    Private m_taskFactory As ITaskFactory
    Private m_eventSaver As IEventSaver

    Public Sub New(ByVal taskFactory As ITaskFactory, ByVal eventSaver As IEventSaver)
        m_taskFactory = taskFactory
        m_eventSaver = eventSaver
    End Sub

    Public Function Create() As IEventTriggerAggregator Implements IEventTriggerFactory.Create
        Dim aggregator As New EventTriggerSaver(m_eventSaver)

        aggregator.Add(New TaskEventTrigger(m_taskFactory))

        Return aggregator
    End Function
End Class
