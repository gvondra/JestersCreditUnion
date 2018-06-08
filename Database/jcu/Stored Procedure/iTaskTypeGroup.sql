CREATE PROCEDURE [jcu].[iTaskTypeGroup]
	@taskTypeId UNIQUEIDENTIFIER,
	@groupId UNIQUEIDENTIFIER,
	@isActive BIT,
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[TaskTypeGroup] ([TaskTypeId], [GroupId], [IsActive], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@taskTypeId, @groupId, @isActive, @timestamp, @timestamp);
END