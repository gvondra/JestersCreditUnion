CREATE TABLE [prd].[CoverageLevel]
(
	[CoverageLevelId] UNIQUEIDENTIFIER NOT NULL,
	[CoverageId] UNIQUEIDENTIFIER NOT NULL,
	[CoverageLimitTypeId] UNIQUEIDENTIFIER NOT NULL,
	[IndividualLimit] DECIMAL(11,2) NULL, 
	[AggregateLimit] DECIMAL(11,2) NULL, 
	[OccurrenceLimit] DECIMAL(11,2) NULL, 
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageLevel_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageLevel_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_CoverageLevel] PRIMARY KEY NONCLUSTERED ([CoverageLevelId]), 
    CONSTRAINT [FK_CoverageLevel_To_Coverage] FOREIGN KEY ([CoverageId]) REFERENCES [prd].[Coverage]([CoverageId]), 
    CONSTRAINT [FK_CoverageLevel_To_CoverageLimitType] FOREIGN KEY ([CoverageLimitTypeId]) REFERENCES [prd].[CoverageLimitType]([CoverageLimitTypeId])
)

GO

CREATE CLUSTERED INDEX [IX_CoverageLevel_CoverageId] ON [prd].[CoverageLevel] ([CoverageId])

GO

CREATE NONCLUSTERED INDEX [IX_CoverageLevel_CoverageLimitTypeId] ON [prd].[CoverageLevel] ([CoverageLimitTypeId])
