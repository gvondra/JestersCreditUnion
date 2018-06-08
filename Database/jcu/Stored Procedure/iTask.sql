CREATE PROCEDURE [jcu].[iTask]
	@id UNIQUEIDENTIFIER OUT, 
	@taskTypeId UNIQUEIDENTIFIER,
	@userId UNIQUEIDENTIFIER,
	@message NVARCHAR(MAX),
	@isClosed BIT,
	@timestamp DATETIME OUT	
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[Task] ([TaskId], [TaskTypeId], [UserId], [Message], [IsClosed], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @taskTypeId, @userId, @message, @isClosed, @timestamp, @timestamp);
END