CREATE TABLE [ln].[PaymentTransaction]
(
	[PaymentId] UNIQUEIDENTIFIER NOT NULL, 
    [TransactionId] UNIQUEIDENTIFIER NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_PaymentTransaction_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_PaymentTransaction] PRIMARY KEY NONCLUSTERED ([PaymentId], [TransactionId]), 
    CONSTRAINT [FK_PaymentTransaction_To_Payment] FOREIGN KEY ([PaymentId]) REFERENCES [ln].[Payment]([PaymentId]), 
    CONSTRAINT [FK_PaymentTransaction_To_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [ln].[Transaction]([TransactionId])
)
