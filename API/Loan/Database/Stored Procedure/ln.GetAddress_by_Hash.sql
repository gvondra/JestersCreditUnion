CREATE PROCEDURE [ln].[GetAddress_By_Hash]
	@hash BINARY(64)
AS
SELECT TOP 1 [AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp]
FROM [ln].[Address]
WHERE [Hash] = @hash
;