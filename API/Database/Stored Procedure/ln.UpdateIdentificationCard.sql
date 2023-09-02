CREATE PROCEDURE [ln].[UpdateIdentificationCard]
	@id UNIQUEIDENTIFIER,
	@initializationVector VARBINARY(16),
	@key VARBINARY(256),
	@masterKeyName VARCHAR(64) = '',
	@timestamp DATETIME2(4) OUT
AS
BEGIN	
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[IdentificationCard]
	SET [InitializationVector] = @initializationVector,
	[Key] = @key,
	[MasterKeyName] = @masterKeyName,
	[UpdateTimestamp] = @timestamp
	WHERE [IdentificationCardId] = @id;
END