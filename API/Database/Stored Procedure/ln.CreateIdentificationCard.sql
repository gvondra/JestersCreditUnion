CREATE PROCEDURE [ln].[CreateIdentificationCard]
	@id UNIQUEIDENTIFIER,
	@initializationVector VARBINARY(16),
	@key VARBINARY(256),
	@timestamp DATETIME2(4) OUT
AS
BEGIN	
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[IdentificationCard] ([IdentificationCardId],[InitializationVector],[Key],
	[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @initializationVector, @key, @timestamp, @timestamp);
END