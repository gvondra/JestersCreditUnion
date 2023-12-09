CREATE TABLE [prd].[Coverage]
(
	[CoverageId] UNIQUEIDENTIFIER NOT NULL,
	[CoverageTypeId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024) NOT NULL,
	[Description] NVARCHAR(5000) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Coverage_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Coverage_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Coverage] PRIMARY KEY NONCLUSTERED ([CoverageId]), 
    CONSTRAINT [FK_Coverage_To_CoverageType] FOREIGN KEY ([CoverageTypeId]) REFERENCES [prd].[CoverageType]([CoverageTypeId])
)

GO

CREATE CLUSTERED INDEX [IX_Coverage_CoverageTypeId] ON [prd].[Coverage] ([CoverageTypeId])
