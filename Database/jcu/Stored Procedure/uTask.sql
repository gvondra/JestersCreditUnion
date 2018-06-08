CREATE PROCEDURE [jcu].[uTask]
	@id UNIQUEIDENTIFIER, 
	@userId UNIQUEIDENTIFIER,
	@message NVARCHAR(MAX),
	@isClosed BIT,
	@timestamp DATETIME OUT	
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[Task] 
	SET [UserId] = @userId,
		[Message] = @message,
		[IsClosed] = @isClosed,
		[UpdateTimestamp] = @timestamp
	WHERE [TaskId] = @id;
END