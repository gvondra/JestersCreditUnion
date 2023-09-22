CREATE TABLE [lnrpt].[LoanBalance]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL, 
	[Date] DATE NOT NULL,
    [Balance] DECIMAL(11,2) NOT NULL,
	[LoanId] BIGINT NOT NULL,
	[LoanAgreementId] BIGINT NOT NULL,
	[LoanStatus] SMALLINT NOT NULL,
	CONSTRAINT [PK_LoanBalance] PRIMARY KEY NONCLUSTERED ([Id]), 
    CONSTRAINT [FK_LoanBalance_Loan] FOREIGN KEY ([LoanId]) REFERENCES [lnrpt].[Loan]([LoanId]), 
    CONSTRAINT [FK_LoanBalance_LoanAgreement] FOREIGN KEY ([LoanAgreementId]) REFERENCES [lnrpt].[LoanAgreement]([LoanAgreementId]), 
    CONSTRAINT [FK_LoanBalance_LoanStatus] FOREIGN KEY ([LoanStatus]) REFERENCES [lnrpt].[LoanStatus]([Status])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanBalance_LoanId] ON [lnrpt].[LoanBalance] ([LoanId] DESC)

GO

CREATE NONCLUSTERED INDEX [IX_LoanBalance_LoanAgreementId] ON [lnrpt].[LoanBalance] ([LoanAgreementId] DESC)

GO

CREATE NONCLUSTERED INDEX [IX_LoanBalance_Status] ON [lnrpt].[LoanBalance] ([LoanStatus])
