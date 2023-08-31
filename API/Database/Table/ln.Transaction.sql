CREATE TABLE [ln].[Transaction]
(
	[TransactionId] UNIQUEIDENTIFIER NOT NULL,
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[Date] DATE NOT NULL,
	[Type] SMALLINT NOT NULL,
	[Amount] DECIMAL(11, 2) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Transaction_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Transaction] PRIMARY KEY NONCLUSTERED ([TransactionId]), 
    CONSTRAINT [FK_Transaction_To_Loan] FOREIGN KEY ([LoanId]) REFERENCES [ln].[Loan]([LoanId])
)

GO

CREATE CLUSTERED INDEX [IX_Transaction_LoanId] ON [ln].[Transaction] ([LoanId])
