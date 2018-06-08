CREATE PROCEDURE [jcu].[sEventTypeAll]
AS
SELECT [EventTypeId], [Title], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[EventType];
