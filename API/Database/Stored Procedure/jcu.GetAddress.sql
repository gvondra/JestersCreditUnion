CREATE PROCEDURE [jcu].[GetAddress]
	@id UNIQUEIDENTIFIER
AS
SELECT [AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp]
FROM [jcu].[Address]
WHERE [AddressId] = @id
;