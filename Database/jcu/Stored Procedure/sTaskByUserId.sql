CREATE PROCEDURE [jcu].[sTaskByUserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT [TaskId], [TaskTypeId], [UserId], [Message], [IsClosed], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Task]
WHERE [UserId] = @userId
ORDER BY [CreateTimestamp]
;

