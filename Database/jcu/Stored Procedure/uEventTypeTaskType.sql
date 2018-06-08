CREATE PROCEDURE [jcu].[uEventTypeTaskType]
	@eventTypeId SMALLINT,
	@taskTypeId UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME OUT	
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[EventTypeTaskType]
	SET [IsActive] = @isActive,
		[UpdateTimestamp] = @timestamp
	WHERE [EventTypeId] = @eventTypeId
		AND [TaskTypeId] = @taskTypeId;
END