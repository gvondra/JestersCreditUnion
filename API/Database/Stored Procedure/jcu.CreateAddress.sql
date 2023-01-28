CREATE PROCEDURE [jcu].[CreateAddress]
	@id UNIQUEIDENTIFIER OUT,
	@hash BINARY(64),
	@recipient VARCHAR(512),
	@attention VARCHAR(512),
	@delivery VARCHAR(512),
	@secondary VARCHAR(512),
	@city VARCHAR(512),
	@state CHAR(2),
	@postalCode VARCHAR(9),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	DECLARE @existingId UNIQUEIDENTIFIER;
	SET @timestamp = NULL;
	SELECT TOP 1 @existingId = [AddressId], @timestamp = [CreateTimestamp] 
	FROM [jcu].[Address] WITH(READUNCOMMITTED)
	WHERE [Hash] = @hash
	AND [Recipient] = @recipient
	AND [Attention] = @attention
	AND [Delivery] = @delivery
	AND [Secondary] = @secondary
	AND [City] = @city
	AND [State] = @state
	AND [PostalCode] = @postalCode
	ORDER BY [CreateTimestamp]
	;
	IF (@timestamp IS NULL AND @existingId IS NULL)
	BEGIN
		IF (@id IS NULL) SET @id = NEWID();			
		SET @timestamp = SYSUTCDATETIME();
		INSERT INTO [jcu].[Address] ([AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp]) 
		VALUES (@id, @hash, @recipient, @attention, @delivery, @secondary, @city, @state, @postalCode, @timestamp)
		;
	END
	IF (@existingId IS NOT NULL)
	BEGIN
		SET @id = @existingId;
	END
END