CREATE PROCEDURE [jcu].[iEventForm]
	@eventId UNIQUEIDENTIFIER,
	@formId UNIQUEIDENTIFIER,
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[EventForm] ([EventId], [FormId], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@eventId, @formId, @timestamp, @timestamp);
END