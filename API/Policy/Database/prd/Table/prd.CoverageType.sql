-- defines the categories for different types of coverages (liability, physical damage, no fault)
CREATE TABLE [prd].[CoverageType]
(
	[CoverageTypeId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024) NOT NULL,
	[Description] NVARCHAR(5000) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageType_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_CoverageType_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_CoverageType] PRIMARY KEY CLUSTERED ([CoverageTypeId])
)
