CREATE PROCEDURE [ln].[CreateIdentificationCard]
	@id UNIQUEIDENTIFIER,
	@initializationVector VARBINARY(16),
	@key VARBINARY(256),
	@masterKeyName VARCHAR(64) = '',
	@timestamp DATETIME2(4) OUT
AS
BEGIN	
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[IdentificationCard] ([IdentificationCardId],[InitializationVector],[Key],[MasterKeyName],
	[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @initializationVector, @key, @masterKeyName,
	@timestamp, @timestamp);
END