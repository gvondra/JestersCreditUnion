CREATE PROCEDURE [ln].[GetAddress]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 
  [AddressId], [Hash], [Recipient], [Attention], [Delivery], [Secondary], [City], [State], [PostalCode], [CreateTimestamp]
FROM [ln].[Address]
WHERE [AddressId] = @id
;