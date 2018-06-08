CREATE PROCEDURE [jcu].[uTaskType]
	@id UNIQUEIDENTIFIER, 
    @title NVARCHAR(100),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[TaskType]
	SET [Title] = @title,
		[UpdateTimestamp] = @timestamp
	WHERE [TaskTypeId] = @id;
END