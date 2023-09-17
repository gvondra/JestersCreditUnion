CREATE TABLE [ln].[LoanHistory]
(
	[LoanHistoryId] UNIQUEIDENTIFIER NOT NULL,
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[InitialDisbursementDate] DATE NULL,
	[FirstPaymentDue] DATE NULL,
	[NextPaymentDue] DATE NULL,
	[Status] SMALLINT NOT NULL,
	[Balance] DECIMAL(11,2) NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanHistory_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanHistory_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanHistory] PRIMARY KEY NONCLUSTERED ([LoanHistoryId]), 
    CONSTRAINT [FK_LoanHistory_Loan] FOREIGN KEY ([LoanId]) REFERENCES [ln].[Loan]([LoanId]), 
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanHistory_LoanId] ON [ln].[LoanHistory] ([LoanId])

GO

CREATE NONCLUSTERED INDEX [IX_LoanHistory_CreateTimestamp_LoanId] ON [ln].[LoanHistory] ([CreateTimestamp] DESC, [LoanId])
