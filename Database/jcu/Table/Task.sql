CREATE TABLE [jcu].[Task]
(
	[TaskId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [TaskTypeId] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NULL, 
    [Message] NVARCHAR(MAX) NOT NULL DEFAULT (''), 
    [IsClosed] BIT NOT NULL DEFAULT (0), 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [FK_Task_To_TaskType] FOREIGN KEY ([TaskTypeId]) REFERENCES [jcu].[TaskType]([TaskTypeId])
)

GO

CREATE INDEX [IX_Task_TaskTypeId] ON [jcu].[Task] ([TaskTypeId])

GO

CREATE INDEX [IX_Task_UserId] ON [jcu].[Task] ([UserId]) WHERE [UserId] is not NULL
