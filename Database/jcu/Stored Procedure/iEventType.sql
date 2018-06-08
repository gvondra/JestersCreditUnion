CREATE PROCEDURE [jcu].[iEventType]
	@id SMALLINT,
	@title NVARCHAR(250),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[EventType] ([EventTypeId], [Title], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @title, @timestamp, @timestamp);
END