CREATE TABLE [ln].[Payment]
(
	[PaymentId] UNIQUEIDENTIFIER NOT NULL,
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[TransactionNumber] VARCHAR(128) NOT NULL,
	[Date] DATE NOT NULL,
	[Amount] DECIMAL(8, 2) NOT NULL,
	[Status] SMALLINT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Payment_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Payment_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Payment] PRIMARY KEY NONCLUSTERED ([PaymentId]), 
    CONSTRAINT [FK_Payment_To_Loan] FOREIGN KEY ([LoanId]) REFERENCES [ln].[Loan]([LoanId])
)

GO

CREATE NONCLUSTERED INDEX [IX_Payment_Status] ON [ln].[Payment] ([Status])

GO

CREATE UNIQUE CLUSTERED INDEX [IX_Payment_Date_LoanId_TransactionNumber] ON [ln].[Payment] ([Date] DESC, [LoanId], [TransactionNumber])

GO

CREATE INDEX [IX_Payment_LoanId] ON [ln].[Payment] ([LoanId])
