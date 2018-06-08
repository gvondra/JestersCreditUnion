CREATE PROCEDURE [jcu].[iEvent]
	@id UNIQUEIDENTIFIER OUT, 
    @eventTypeId SMALLINT, 
	@message NVARCHAR(MAX),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[Event] ([EventId], [EventTypeId], [Message], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @eventTypeId, @message, @timestamp, @timestamp);
END