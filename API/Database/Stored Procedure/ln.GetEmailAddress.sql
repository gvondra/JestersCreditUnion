CREATE PROCEDURE [ln].[GetEmailAddress]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 [EmailAddressId], [Address], [CreateTimestamp]
FROM [ln].[EmailAddress]
WHERE [EmailAddressId] = @id
;
