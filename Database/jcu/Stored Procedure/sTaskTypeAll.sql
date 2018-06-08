CREATE PROCEDURE [jcu].[sTaskTypeAll]
AS
SELECT [TaskTypeId], [Title], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[TaskType];
