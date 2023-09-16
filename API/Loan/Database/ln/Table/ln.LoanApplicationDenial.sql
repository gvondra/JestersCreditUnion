CREATE TABLE [ln].[LoanApplicationDenial]
(
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Reason] SMALLINT NOT NULL,
	[Date] DATE NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplicationDenial_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL, 
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplicationDenial_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL, 
	CONSTRAINT [PK_LoanApplicationDenial] PRIMARY KEY NONCLUSTERED ([LoanApplicationId]), 
    CONSTRAINT [FK_LoanApplicationDenial_To_LoanApplication] FOREIGN KEY ([LoanApplicationId]) REFERENCES [ln].[LoanApplication]([LoanApplicationId])
)
