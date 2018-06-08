CREATE PROCEDURE [jcu].[sTask]
	@taskId as UNIQUEIDENTIFIER
AS
SELECT [TaskId], [TaskTypeId], [UserId], [Message], [IsClosed], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Task]
WHERE [TaskId] = @taskId
;
