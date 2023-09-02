CREATE PROCEDURE [ln].[CreateEmailAddress]
	@id UNIQUEIDENTIFIER OUT,
	@address VARCHAR(1024),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[EmailAddress] ([EmailAddressId], [Address], [CreateTimestamp])
	VALUES (@id, @address, @timestamp);
END