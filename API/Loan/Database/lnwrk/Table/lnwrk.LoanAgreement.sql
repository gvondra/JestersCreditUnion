CREATE TABLE [lnwrk].[LoanAgreement]
(
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[Hash] BINARY(64) NOT NULL,
	[CreateDate] DATE NOT NULL,
	[AgreementDate] DATE NULL,
	[InterestRate] DECIMAL(5, 4) NOT NULL,
	[PaymentAmount] DECIMAL(7, 2) NOT NULL
)
