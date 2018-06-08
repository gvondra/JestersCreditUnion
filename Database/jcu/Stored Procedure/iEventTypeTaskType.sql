CREATE PROCEDURE [jcu].[iEventTypeTaskType]
	@eventTypeId SMALLINT,
	@taskTypeId UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME OUT	
AS
BEGIN
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[EventTypeTaskType]  ([EventTypeId], [TaskTypeId], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@eventTypeId, @taskTypeId, @isActive, @timestamp, @timestamp);
END
