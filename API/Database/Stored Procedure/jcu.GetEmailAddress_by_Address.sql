CREATE PROCEDURE [jcu].[GetEmailAddress_by_Address]
	@address VARCHAR(1024)
AS
SELECT [EmailAddressId], [Address], [CreateTimestamp]
FROM [jcu].[EmailAddress]
WHERE [Address] = @address
;