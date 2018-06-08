CREATE TABLE [jcu].[Event]
(
	[EventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [EventTypeId] SMALLINT NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL DEFAULT (''), 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate()
)
