CREATE PROCEDURE [jcu].[CreatePhone]
	@id UNIQUEIDENTIFIER OUT,
	@number VARCHAR(10),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	DECLARE @existingId UNIQUEIDENTIFIER;
	SET @timestamp = NULL;
	SELECT TOP 1 @existingId = [PhoneId], @timestamp = [CreateTimestamp] 
	FROM [jcu].[Phone] WITH(READUNCOMMITTED)
	WHERE [Number] = @number
	ORDER BY [CreateTimestamp]
	;
	IF @existingId IS NULL AND @timestamp IS NULL
	BEGIN
		IF @id IS NULL SET @id = NEWID();
		SET @timestamp = SYSUTCDATETIME();
		INSERT INTO [jcu].[Phone] ([PhoneId], [Number], [CreateTimestamp])
		VALUES (@id, @number, @timestamp)
		;
	END
	IF @existingId IS NOT NULL 
	BEGIN
		SET @id = @existingId;
	END
END