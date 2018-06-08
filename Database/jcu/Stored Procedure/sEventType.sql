CREATE PROCEDURE [jcu].[sEventType]
	@id SMALLINT
AS
SELECT [EventTypeId], [Title], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[EventType]
WHERE [EventTypeId] = @id;
