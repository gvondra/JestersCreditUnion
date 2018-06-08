CREATE PROCEDURE [jcu].[sUnassignedTaskByUser]
	@userId UNIQUEIDENTIFIER
AS
SELECT [TaskId], [TaskTypeId], [Message], [CreateTimestamp], [UpdateTimestamp],
		[TaskTypeTitle],
		[GroupId], [GroupName]
FROM [jcu].[UnassignedTask]
WHERE [GroupId] is Null
	OR [GroupId] in (
		SELECT [UG].[GroupId]
		FROM [jcu].[UserGroup] [UG]
		WHERE [UG].[UserId] = @userId
	)
ORDER BY [UpdateTimestamp]
;