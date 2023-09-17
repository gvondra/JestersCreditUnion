CREATE TABLE [lnwrk].[LoanBalance]
(
	[Timestamp] DATETIME2(4) NOT NULL,
	[Date] DATE NOT NULL,
    [Balance] DECIMAL(11,2) NULL,
	[LoanId] BIGINT NOT NULL,
	[LoanAgreementId] BIGINT NOT NULL,
	[LoanStatus] SMALLINT NOT NULL
)
