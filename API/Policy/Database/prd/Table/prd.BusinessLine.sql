CREATE TABLE [prd].[BusinessLine]
(
	[BusinessLineId] UNIQUEIDENTIFIER NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_BusinessLine_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_BusinessLine_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_BusinessLine] PRIMARY KEY CLUSTERED ([BusinessLineId])
)
