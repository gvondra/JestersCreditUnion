CREATE TABLE [prd].[Product]
(
	[ProductId] UNIQUEIDENTIFIER NOT NULL,
	[BusinessLineId] UNIQUEIDENTIFIER NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Product_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Product_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY NONCLUSTERED ([ProductId]), 
    CONSTRAINT [FK_Product_To_BusinessLine] FOREIGN KEY ([BusinessLineId]) REFERENCES [prd].[BusinessLine]([BusinessLineId])
)

GO

CREATE CLUSTERED INDEX [IX_Product_BusinessLineId] ON [prd].[Product] ([BusinessLineId])
