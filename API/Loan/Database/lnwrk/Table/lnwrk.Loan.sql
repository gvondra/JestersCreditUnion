CREATE TABLE [lnwrk].[Loan]
(
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[Number] VARCHAR(16) NOT NULL,
	[InitialDisbursementDate] DATE NULL,
	[FirstPaymentDue] DATE NULL,
	[NextPaymentDue] DATE NULL	
)
