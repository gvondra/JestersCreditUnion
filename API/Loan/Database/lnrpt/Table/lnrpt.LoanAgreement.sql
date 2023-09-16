CREATE TABLE [lnrpt].[LoanAgreement]
(
	[LoanAgreementId] BIGINT IDENTITY(1,1) NOT NULL,
	[CreateDate] DATE NOT NULL,
	[AgreementDate] DATE NULL,
	[InterestRate] DECIMAL(5, 4) NOT NULL,
	[PaymentAmount] DECIMAL(7, 2) NOT NULL,
	CONSTRAINT [PK_LoanAgrement] PRIMARY KEY NONCLUSTERED ([LoanAgreementId])
)
