CREATE PROCEDURE [jcu].[GetPhone_by_Number]
	@number VARCHAR(10)
AS
SELECT [PhoneId], [Number], [CreateTimestamp]
FROM [jcu].[Phone] 
WHERE [Number] = @number
;