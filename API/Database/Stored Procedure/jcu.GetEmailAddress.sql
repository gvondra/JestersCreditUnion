CREATE PROCEDURE [jcu].[GetEmailAddress]
	@id UNIQUEIDENTIFIER
AS
SELECT [EmailAddressId], [Address], [CreateTimestamp]
FROM [jcu].[EmailAddress]
WHERE [EmailAddressId] = @id
;