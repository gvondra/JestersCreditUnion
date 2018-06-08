CREATE TABLE [jcu].[TaskType]
(
	[TaskTypeId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(100) NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate()
)
