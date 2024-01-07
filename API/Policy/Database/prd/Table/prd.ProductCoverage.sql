CREATE TABLE [prd].[ProductCoverage]
(
	[ProductId] UNIQUEIDENTIFIER NOT NULL,
	[CoverageId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [PK_ProductCoverage] PRIMARY KEY NONCLUSTERED ([ProductId], [CoverageId]), 
    CONSTRAINT [FK_ProductCoverage_To_Product] FOREIGN KEY ([ProductId]) REFERENCES [prd].[Product]([ProductId]), 
    CONSTRAINT [FK_ProductCoverage_To_Coverage] FOREIGN KEY ([CoverageId]) REFERENCES [prd].[Coverage]([CoverageId])
)

GO

CREATE NONCLUSTERED INDEX [IX_ProductCoverage_ProductId] ON [prd].[ProductCoverage] ([ProductId])

GO

CREATE NONCLUSTERED INDEX [IX_ProductCoverage_CoverageId] ON [prd].[ProductCoverage] ([CoverageId])
