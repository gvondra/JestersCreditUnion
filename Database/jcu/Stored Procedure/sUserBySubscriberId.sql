CREATE PROCEDURE [jcu].[sUserBySubscriberId]
	@subscriberId NVARCHAR(1000)
AS
	SELECT [User].[UserId], [User].[FullName], [User].[ShortName], [User].[BirthDate], [User].[EmailAddress], [User].[PhoneNumber], [User].[CreateTimestamp], [User].[UpdateTimestamp]
	FROM [jcu].[User]
		INNER JOIN [jcu].[UserAccount] on [User].[UserId] = [UserAccount].[UserId]
	WHERE [UserAccount].[SubscriberId] = @subscriberId;
