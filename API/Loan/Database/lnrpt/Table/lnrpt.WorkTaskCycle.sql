CREATE TABLE [lnrpt].[WorkTaskCycle]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[CreateDate] DATE NOT NULL,
	[AssignedDate] DATE NULL,
	[AssignedToUserId] UNIQUEIDENTIFIER NULL,
	[ClosedDate] DATE NULL,
	[WorkTaskId] BIGINT NOT NULL,
	[TypeCode] NVARCHAR(128) NOT NULL,
	[StatusCode] NVARCHAR(128) NOT NULL,
	CONSTRAINT [PK_WorkTaskCycle] PRIMARY KEY NONCLUSTERED ([Id]), 
    CONSTRAINT [FK_WorkTaskCycle_To_WorkTask] FOREIGN KEY ([WorkTaskId]) REFERENCES [lnrpt].[WorkTask]([WorkTaskId]), 
    CONSTRAINT [FK_WorkTaskCycle_To_WorkTaskType] FOREIGN KEY ([TypeCode]) REFERENCES [lnrpt].[WorkTaskType]([Code]), 
    CONSTRAINT [FK_WorkTaskCycle_To_WorkTaskStatus] FOREIGN KEY ([StatusCode]) REFERENCES [lnrpt].[WorkTaskStatus]([Code]), 
    CONSTRAINT [FK_WorkTaskCycle_To_User] FOREIGN KEY ([AssignedToUserId]) REFERENCES [lnrpt].[User]([UserId])
)

GO

CREATE NONCLUSTERED INDEX [IX_WorkTaskCycle_AssignedToUserId] ON [lnrpt].[WorkTaskCycle] ([AssignedToUserId])

GO

CREATE NONCLUSTERED INDEX [IX_WorkTaskCycle_WorkTaskId] ON [lnrpt].[WorkTaskCycle] ([WorkTaskId])

GO

CREATE NONCLUSTERED INDEX [IX_WorkTaskCycle_TypeCode] ON [lnrpt].[WorkTaskCycle] ([TypeCode])

GO

CREATE NONCLUSTERED INDEX [IX_WorkTaskCycle_StatusCode] ON [lnrpt].[WorkTaskCycle] ([StatusCode])

GO

CREATE NONCLUSTERED INDEX [IX_WorkTaskCycle_CreateDate] ON [lnrpt].[WorkTaskCycle] ([CreateDate] DESC)
