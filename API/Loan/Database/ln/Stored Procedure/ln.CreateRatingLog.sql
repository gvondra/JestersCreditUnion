CREATE PROCEDURE [ln].[CreateRatingLog]
	@id UNIQUEIDENTIFIER OUT,
	@ratingId UNIQUEIDENTIFIER,
	@value REAL,
	@description VARCHAR(MAX),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[RatingLog] ([RatingLogId], [RatingId], [Value], [Description], [CreateTimestamp])
	VALUES (@id, @ratingId, @value, @description, @timestamp);
END