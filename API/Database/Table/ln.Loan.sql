CREATE TABLE [ln].[Loan]
(
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[Number] VARCHAR(16) NOT NULL,
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[InitialDisbursementDate] DATE NULL,
	[FirstPaymentDue] DATE NULL,
	[NextPaymentDue] DATE NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Loan_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Loan_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Loan] PRIMARY KEY NONCLUSTERED ([LoanId]), 
    CONSTRAINT [FK_Loan_To_LoanApplication] FOREIGN KEY ([LoanApplicationId]) REFERENCES [ln].[LoanApplication]([LoanApplicationId]), 
    CONSTRAINT [FK_Loan_To_LoanAgreement] FOREIGN KEY ([LoanId]) REFERENCES [ln].[LoanAgreement]([LoanId])
)

GO

CREATE NONCLUSTERED INDEX [IX_Loan_Number] ON [ln].[Loan] ([Number])

GO

CREATE NONCLUSTERED INDEX [IX_Loan_LoanApplicationId] ON [ln].[Loan] ([LoanApplicationId])
