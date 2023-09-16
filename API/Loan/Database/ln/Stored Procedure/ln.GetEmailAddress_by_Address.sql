CREATE PROCEDURE [ln].[GetEmailAddress_by_Address]
	@address VARCHAR(1024)
AS
SELECT TOP 1 [EmailAddressId], [Address], [CreateTimestamp]
FROM [ln].[EmailAddress]
WHERE [Address] = @address
;
