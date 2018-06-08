CREATE PROCEDURE [jcu].[iGroup]
	@id UNIQUEIDENTIFIER OUT, 
    @name NVARCHAR(100),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[Group] ([GroupId], [Name], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @name, @timestamp, @timestamp);
END