CREATE TABLE [ln].[LoanApplicationComment]
(
	[LoanApplicationCommentId] UNIQUEIDENTIFIER NOT NULL,
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[IsInternal] BIT CONSTRAINT [DF_LoanApplicationComment] DEFAULT (1) NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplicationComment_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanApplicationComment] PRIMARY KEY NONCLUSTERED ([LoanApplicationCommentId]), 
    CONSTRAINT [FK_LoanApplicationComment_To_LoanApplication] FOREIGN KEY ([LoanApplicationId]) REFERENCES [ln].[LoanApplication]([LoanApplicationId])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanApplicationComment_LoanApplicationId] ON [ln].[LoanApplicationComment] ([LoanApplicationId])
