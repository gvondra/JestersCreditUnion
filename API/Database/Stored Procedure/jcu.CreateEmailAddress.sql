CREATE PROCEDURE [jcu].[CreateEmailAddress]
	@id UNIQUEIDENTIFIER OUT,
	@address VARCHAR(1024),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	DECLARE @existingId UNIQUEIDENTIFIER;
	SET @timestamp = NULL;
	SELECT TOP 1 @existingId = [EmailAddressId], @timestamp = [CreateTimestamp] 
	FROM [jcu].[EmailAddress] WITH(READUNCOMMITTED) 
	WHERE [Address] = @address
	;
	IF (@timestamp IS NULL AND @existingId IS NULL)
	BEGIN 
		IF (@id IS NULL) SET @id = NEWID();
		SET @timestamp = SYSUTCDATETIME();
		INSERT INTO [jcu].[EmailAddress] ([EmailAddressId], [Address], [CreateTimestamp])
		VALUES (@id, @address, @timestamp)
		;
	END
	IF (@existingId IS NOT NULL)
	BEGIN
		SET @id = @existingId;
	END
END