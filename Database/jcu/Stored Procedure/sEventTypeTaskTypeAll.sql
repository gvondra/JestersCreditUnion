CREATE PROCEDURE [jcu].[sEventTypeTaskTypeAll]
AS
SELECT [EventTypeId], [TaskTypeId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[EventTypeTaskType];
