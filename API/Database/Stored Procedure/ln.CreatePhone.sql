CREATE PROCEDURE [ln].[CreatePhone]
	@id UNIQUEIDENTIFIER OUT,
	@number VARCHAR(15),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Phone] ([PhoneId], [Number], [CreateTimestamp])
	VALUES (@id, @number, @timestamp);
END