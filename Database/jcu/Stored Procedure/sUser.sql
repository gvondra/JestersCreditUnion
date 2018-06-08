CREATE PROCEDURE [jcu].[sUser]
	@userId UNIQUEIDENTIFIER
AS
SELECT [UserId], [FullName], [ShortName], [BirthDate], [EmailAddress], [PhoneNumber], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[User]
WHERE [UserId] = @userId;
