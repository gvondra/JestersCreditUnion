CREATE PROCEDURE [ln].[UpdateIdentificationCard]
	@id UNIQUEIDENTIFIER,
	@initializationVector VARBINARY(16),
	@key VARBINARY(256),
	@timestamp DATETIME2(4) OUT
AS
BEGIN	
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[IdentificationCard]
	SET [InitializationVector] = @initializationVector,
	[Key] = @key,
	[UpdateTimestamp] = @timestamp
	WHERE [IdentificationCardId] = @id;
END