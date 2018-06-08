CREATE VIEW [jcu].[UnassignedTask]
	AS SELECT [Task].[TaskId], [Task].[TaskTypeId], [Task].[Message], [Task].[IsClosed], [Task].[CreateTimestamp], [Task].[UpdateTimestamp],
		[TT].[Title] [TaskTypeTitle],
		[Group].[GroupId], COALESCE([Group].[Name], 'Ungrouped') [GroupName]
	FROM [jcu].[Task]
		INNER JOIN [jcu].[TaskType] [TT] on [Task].[TaskTypeId] = [TT].[TaskTypeId]
		LEFT JOIN (
			[jcu].[TaskTypeGroup] [TTG]
			INNER JOIN [jcu].[Group] on [TTG].IsActive = 1 AND [TTG].[GroupId] = [Group].[GroupId]
		) ON [TT].[TaskTypeId] = [TTG].[TaskTypeId]
	WHERE [Task].[UserId] is Null
		AND [Task].[IsClosed] = 0;
