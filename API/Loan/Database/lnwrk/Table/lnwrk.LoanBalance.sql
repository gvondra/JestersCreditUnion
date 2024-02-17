CREATE TABLE [lnwrk].[LoanBalance]
(
	[Timestamp] DATETIME2(4) NOT NULL,
	[Date] DATE NOT NULL,
    [Balance] DECIMAL(11,2) NULL,
	[DaysPastDue] SMALLINT NULL,
	[LoanStatus] SMALLINT NOT NULL,
	[Number] VARCHAR(16) NOT NULL,
	[InitialDisbursementDate] DATE NULL,
	[FirstPaymentDue] DATE NULL,
	[NextPaymentDue] DATE NULL,
	[AgreementHash] BINARY(64) NOT NULL,
	[AgreementCreateDate] DATE NOT NULL,
	[AgreementDate] DATE NULL,
	[InterestRate] DECIMAL(5, 4) NOT NULL,
	[PaymentAmount] DECIMAL(7, 2) NOT NULL
)
