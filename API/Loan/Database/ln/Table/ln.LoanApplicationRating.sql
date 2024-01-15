CREATE TABLE [ln].[LoanApplicationRating]
(
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[RatingId] UNIQUEIDENTIFIER NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplicationRating_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplicationRating_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanApplicationRating] PRIMARY KEY CLUSTERED ([LoanApplicationId]), 
    CONSTRAINT [FK_LoanApplicationRating_To_LoanApplication] FOREIGN KEY ([LoanApplicationId]) REFERENCES [ln].[LoanApplication]([LoanApplicationId]), 
    CONSTRAINT [FK_LoanApplicationRating_To_Rating] FOREIGN KEY ([RatingId]) REFERENCES [ln].[Rating]([RatingId])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanApplicationRating_RatingId] ON [ln].[LoanApplicationRating] ([RatingId])
