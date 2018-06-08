CREATE TABLE [jcu].[EventType]
(
	[EventTypeId] SMALLINT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(250) NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate()
)
