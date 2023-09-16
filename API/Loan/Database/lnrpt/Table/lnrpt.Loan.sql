CREATE TABLE [lnrpt].[Loan]
(
	[LoanId] BIGINT IDENTITY(1,1) NOT NULL,
	[Number] VARCHAR(16) NOT NULL,
	[InitialDisbursementDate] DATE NULL,
	[FirstPaymentDue] DATE NULL,
	[NextPaymentDue] DATE NULL,
	CONSTRAINT [PK_Loan] PRIMARY KEY NONCLUSTERED ([LoanId])
)

GO

CREATE NONCLUSTERED INDEX [IX_Loan_Number] ON [lnrpt].[Loan] ([Number])
