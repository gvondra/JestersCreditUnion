﻿CREATE TABLE [lnrpt].[WorkTask]
(
	[WorkTaskId] BIGINT IDENTITY(1,1) NOT NULL,
	[Title] NVARCHAR(512) NOT NULL,
	CONSTRAINT [PK_WorkTask] PRIMARY KEY CLUSTERED ([WorkTaskId])
)
