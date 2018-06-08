CREATE PROCEDURE [jcu].[sEventTypeTaskTypeByEventTypeId]
	@eventTypeId SMALLINT
AS
BEGIN
	SELECT [EventTypeId], [TaskTypeId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
	FROM [jcu].[EventTypeTaskType]
	WHERE [EventTypeId] = @eventTypeId;

	SELECT [TT].[TaskTypeId], [TT].[Title], [TT].[CreateTimestamp], [TT].[UpdateTimestamp]
	FROM [jcu].[TaskType] [TT]
		INNER JOIN [jcu].[EventTypeTaskType] [ETTT] on [TT].[TaskTypeId] = [ETTT].[TaskTypeId]
	WHERE [ETTT].[EventTypeId] = @eventTypeId;
END
