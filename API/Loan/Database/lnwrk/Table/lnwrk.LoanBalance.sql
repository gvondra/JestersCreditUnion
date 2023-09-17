CREATE TABLE [lnwrk].[LoanBalance]
(
	[Date] DATE NOT NULL,
    [Balance] DECIMAL(11,2) NOT NULL,
	[LoanId] BIGINT NOT NULL,
	[LoanAgreementId] BIGINT NOT NULL,
	[LoanStatus] SMALLINT NOT NULL
)
