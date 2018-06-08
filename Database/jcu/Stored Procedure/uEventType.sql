CREATE PROCEDURE [jcu].[uEventType]
	@id SMALLINT,
	@title NVARCHAR(250),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[EventType]
	SET [Title] = @title,
		[UpdateTimestamp] = @timestamp
	WHERE [EventTypeId] = @id;
END