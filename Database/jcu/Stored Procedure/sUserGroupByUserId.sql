CREATE PROCEDURE [jcu].[sUserGroupByUserId]
	@userId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT [UserId], [GroupId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
	FROM [jcu].[UserGroup]
	WHERE [UserId] = @userId;

	SELECT [Gr].[GroupId], [Gr].[Name], [Gr].[CreateTimestamp], [Gr].[UpdateTimestamp]
	FROM [jcu].[Group] [Gr]
		INNER JOIN [jcu].[UserGroup] [UG] on [Gr].[GroupId] = [UG].[GroupId]
	WHERE [UG].[UserId] = @userId;
END
