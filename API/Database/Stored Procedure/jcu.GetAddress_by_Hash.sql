CREATE PROCEDURE [jcu].[GetAddress_by_Hash]
	@hash BINARY(64)
AS
SELECT [AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp]
FROM [jcu].[Address]
WHERE [Hash] = @hash
ORDER BY [CreateTimestamp]
;