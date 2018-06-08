CREATE PROCEDURE [jcu].[uTaskTypeGroup]
	@taskTypeId UNIQUEIDENTIFIER,
	@groupId UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[TaskTypeGroup]
	SET [IsActive] = @isActive,
		[UpdateTimestamp] = @timestamp
	WHERE [TaskTypeId] = @taskTypeId
		AND [GroupId] = @groupId;
END