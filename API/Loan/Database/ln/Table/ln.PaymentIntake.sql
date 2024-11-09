CREATE TABLE [ln].[PaymentIntake]
(
	[PaymentIntakeId] UNIQUEIDENTIFIER NOT NULL,
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[PaymentId] UNIQUEIDENTIFIER NULL,
	[TransactionNumber] VARCHAR(128) NOT NULL,
	[Date] DATE NOT NULL,
	[Amount] DECIMAL(8, 2) NOT NULL,
	[Status] SMALLINT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_PaymentIntake_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_PaymentIntake_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[CreateUserId] VARCHAR(64) NOT NULL,
	[UpdateUserId] VARCHAR(64) NOT NULL,
	CONSTRAINT [PK_PaymentIntake] PRIMARY KEY CLUSTERED ([PaymentIntakeId]), 
    CONSTRAINT [FK_PaymentIntake_To_Loan] FOREIGN KEY ([LoanId]) REFERENCES [ln].[Loan]([LoanId]), 
    CONSTRAINT [FK_PaymentIntake_To_Payment] FOREIGN KEY ([PaymentId]) REFERENCES [ln].[Payment]([PaymentId])
)

GO

CREATE NONCLUSTERED INDEX [IX_PaymentIntake_LoanId] ON [ln].[PaymentIntake] ([LoanId])

GO

CREATE NONCLUSTERED INDEX [IX_PaymentIntake_PaymentId] ON [ln].[PaymentIntake] ([PaymentId])

GO

CREATE NONCLUSTERED INDEX [IX_PaymentIntake_Status_UpdateTimestamp] ON [ln].[PaymentIntake] ([Status], [UpdateTimestamp] DESC)
