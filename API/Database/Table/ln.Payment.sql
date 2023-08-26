CREATE TABLE [ln].[Payment]
(
	[PaymentId] UNIQUEIDENTIFIER NOT NULL,
	[LoanNumber] VARCHAR(128) NOT NULL,
	[TransactionNumber] VARCHAR(128) NOT NULL,
	[Date] DATE NOT NULL,
	[Amount] DECIMAL(8, 2) NOT NULL,
	[Status] SMALLINT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Payment_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_Payment_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Payment] PRIMARY KEY NONCLUSTERED ([PaymentId])
)

GO

CREATE NONCLUSTERED INDEX [IX_Payment_Status] ON [ln].[Payment] ([Status])

GO

CREATE UNIQUE CLUSTERED INDEX [IX_Payment_Date_LoanNumber_TransactionNumber] ON [ln].[Payment] ([Date] DESC, [LoanNumber], [TransactionNumber])
