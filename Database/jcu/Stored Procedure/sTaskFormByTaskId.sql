CREATE PROCEDURE [jcu].[sTaskFormByTaskId]
	@taskId UNIQUEIDENTIFIER
AS
SELECT [FormId]
FROM [jcu].[TaskForm]
WHERE [TaskId] = @taskId;
