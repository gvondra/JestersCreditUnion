CREATE PROCEDURE [jcu].[sUserByEmailAddress]
	@emailAddress NVARCHAR(500)
AS
	SELECT [UserId], [FullName], [ShortName], [BirthDate], [EmailAddress], [PhoneNumber], [CreateTimestamp], [UpdateTimestamp]
	FROM [jcu].[User]
	WHERE [EmailAddress] = @emailAddress;
