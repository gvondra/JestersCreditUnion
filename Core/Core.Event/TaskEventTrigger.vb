Public Class TaskEventTrigger
    Implements IEventTrigger

    Private m_taskFactory As ITaskFactory

    Public Sub New(ByVal taskFactory As ITaskFactory)
        m_taskFactory = taskFactory
    End Sub

    Public Sub Trigger(settings As ISettings, [event] As IEvent) Implements IEventTrigger.Trigger
        Dim taskType As ITaskType
        Dim task As ITask

        For Each taskType In [event].GetEventType(settings).GetTaskTypes(settings).Where(Function(ettt As IEventTypeTaskType) ettt.IsActive)
            task = m_taskFactory.Create(taskType)
            task.Message = [event].Message
            task = [event].AddTask(task)
        Next
    End Sub
End Class
