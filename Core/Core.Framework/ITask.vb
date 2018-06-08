Public Interface ITask
    Inherits ISavable

    ReadOnly Property TaskId As Guid
    Property Message As String
    Property IsClosed As Boolean
    ReadOnly Property CreateTimestamp As Date

    Function GetUser(ByVal settings As ISettings) As IUser
    Sub SetUser(ByVal user As IUser)
    Function GetTaskType(ByVal settings As ISettings) As ITaskType
End Interface
