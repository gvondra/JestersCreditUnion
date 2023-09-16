CREATE PROCEDURE [ln].[GetPhone]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 [PhoneId], [Number], [CreateTimestamp]
FROM [ln].[Phone]
WHERE [PhoneId] = @id
;