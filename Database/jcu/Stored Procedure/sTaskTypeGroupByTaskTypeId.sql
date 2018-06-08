CREATE PROCEDURE [jcu].[sTaskTypeGroupByTaskTypeId]
	@taskTypeId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT [TaskTypeId], [GroupId], [IsActive], [CreateTimestamp], [UpdateTimestamp]
	FROM [jcu].[TaskTypeGroup]
	WHERE [TaskTypeId] = @taskTypeId;

	SELECT [G].[GroupId], [G].[Name], [G].[CreateTimestamp], [G].[UpdateTimestamp]
	FROM [jcu].[TaskTypeGroup] [TTG]
		INNER JOIN [jcu].[Group] [G] on [TTG].[GroupId] = [G].[GroupId]
	WHERE [TTG].[TaskTypeId] = @taskTypeId;
END
