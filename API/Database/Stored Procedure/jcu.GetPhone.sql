CREATE PROCEDURE [jcu].[GetPhone]
	@id UNIQUEIDENTIFIER
AS
SELECT [PhoneId], [Number], [CreateTimestamp]
FROM [jcu].[Phone] 
WHERE [PhoneId] = @id 
;