﻿CREATE VIEW [jcu].[TaskForm]
	AS SELECT [ET].[TaskId], [EF].[FormId]
	FROM [jcu].[EventTask] [ET]
		INNER JOIN [jcu].[Event] [E] on [ET].[EventId] = [E].[EventId]
		INNER JOIN [jcu].[EventForm] [EF] on [E].[EventId] = [EF].[EventId]
