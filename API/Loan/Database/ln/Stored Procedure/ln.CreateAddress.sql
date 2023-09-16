CREATE PROCEDURE [ln].[CreateAddress]
	@id UNIQUEIDENTIFIER OUT,
	@hash BINARY(64),
	@recipient NVARCHAR(512),
	@attention NVARCHAR(512),
	@delivery NVARCHAR(512),
	@secondary NVARCHAR(512),
	@city NVARCHAR(512),
	@state NCHAR(2),
	@postalCode NVARCHAR(9),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Address] ([AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp])
	VALUES (@id, @hash, @recipient, @attention, @delivery, @secondary, @city, @state, @postalCode, @timestamp);
END