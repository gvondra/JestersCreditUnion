CREATE PROCEDURE [jcu].[iTaskType]
	@id UNIQUEIDENTIFIER OUT, 
    @title NVARCHAR(100),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[TaskType] ([TaskTypeId], [Title], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @title, @timestamp, @timestamp);
END