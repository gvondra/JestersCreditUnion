CREATE PROCEDURE [ln].[GetPhone_by_Number]
	@number VARCHAR(15)
AS
SELECT TOP 1 [PhoneId], [Number], [CreateTimestamp]
FROM [ln].[Phone]
WHERE [Number] = @number
;