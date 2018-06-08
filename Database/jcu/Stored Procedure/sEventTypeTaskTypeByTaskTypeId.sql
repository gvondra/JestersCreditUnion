CREATE PROCEDURE [jcu].[sEventTypeTaskTypeByTaskTypeId]
	@taskTypeId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT [EventTypeId], [TaskTypeId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
	FROM [jcu].[EventTypeTaskType]
	WHERE [TaskTypeId] = @taskTypeId;

	SELECT [ET].[EventTypeId], [ET].[Title], [ET].[CreateTimestamp], [ET].[UpdateTimestamp]
	FROM [jcu].[EventType] [ET]
		INNER JOIN [jcu].[EventTypeTaskType] [ETTT] on [ET].[EventTypeId] = [ETTT].[EventTypeId]
	WHERE [ETTT].[TaskTypeId] = @taskTypeId;
END
