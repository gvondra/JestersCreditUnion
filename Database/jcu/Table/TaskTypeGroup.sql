CREATE TABLE [jcu].[TaskTypeGroup]
(
	[TaskTypeId] UNIQUEIDENTIFIER NOT NULL , 
    [GroupId] UNIQUEIDENTIFIER NOT NULL,
    [IsActive] BIT NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(),
    PRIMARY KEY ([GroupId], [TaskTypeId]), 
    CONSTRAINT [FK_TaskTypeGroup_To_TaskType] FOREIGN KEY ([TaskTypeId]) REFERENCES [jcu].[TaskType]([TaskTypeId]), 
    CONSTRAINT [FK_TaskTypeGroup_To_Group] FOREIGN KEY ([GroupId]) REFERENCES [jcu].[Group]([GroupId])
)

GO

CREATE INDEX [IX_TaskTypeGroup_TaskTypeId] ON [jcu].[TaskTypeGroup] ([TaskTypeId])

GO

CREATE INDEX [IX_TaskTypeGroup_GroupId] ON [jcu].[TaskTypeGroup] ([GroupId])
