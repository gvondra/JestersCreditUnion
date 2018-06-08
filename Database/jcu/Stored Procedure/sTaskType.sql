CREATE PROCEDURE [jcu].[sTaskType]
	@id UNIQUEIDENTIFIER
AS
SELECT [TaskTypeId], [Title], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[TaskType]
WHERE [TaskTypeId] = @id;