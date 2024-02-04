CREATE PROCEDURE [ln].[CreateRating]
	@id UNIQUEIDENTIFIER OUT,
	@value REAL,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Rating] ([RatingId], [Value], [CreateTimestamp])
	VALUES (@id, @value, @timestamp);
END