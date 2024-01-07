-- Not sure what this table is used for
-- the category of indemnification limit that is applied under a coverage
CREATE TABLE [prd].[CoverageLimitType]
(
	[CoverageLimitTypeId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024) NOT NULL,
	[Description] NVARCHAR(5000) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageLimitType_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageLimitType_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_CoverageLimitType] PRIMARY KEY CLUSTERED ([CoverageLimitTypeId])
)
