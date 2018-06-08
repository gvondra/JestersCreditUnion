CREATE PROCEDURE [jcu].[sUserSearch]
	@value NVARCHAR(500),
	@wildCardValue NVARCHAR(500)
AS
SELECT [UserId], [FullName], [ShortName], [BirthDate], [EmailAddress], [PhoneNumber], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[User]
WHERE [EmailAddress] = @value
	OR [FullName] LIKE @wildCardValue ESCAPE '\'
	OR [ShortName] LIKE @wildCardValue ESCAPE '\';