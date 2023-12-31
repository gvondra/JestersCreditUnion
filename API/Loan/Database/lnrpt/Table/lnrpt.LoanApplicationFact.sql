CREATE TABLE [lnrpt].[LoanApplicationFact]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[ApplicationDate] DATE NOT NULL,
	[ClosedDate] DATE NULL,
	[Amount] DECIMAL(11, 2) NOT NULL,
	[Count] INT CONSTRAINT [DF_LoanApplicationFact_Count] DEFAULT 1 NOT NULL, -- default added to allow the column to be added to existing tables having data
																			  -- long term, the default constraint can be removed
	[Status] SMALLINT NOT NULL,
	[UserId] UNIQUEIDENTIFIER NULL,
	CONSTRAINT [PK_LoanApplicationFact] PRIMARY KEY NONCLUSTERED ([Id]), 
    CONSTRAINT [FK_LoanApplicationFact_To_LoanApplicationStatus] FOREIGN KEY ([Status]) REFERENCES [lnrpt].[LoanApplicationStatus]([Status]), 
    CONSTRAINT [FK_LoanApplicationFact_To_User] FOREIGN KEY ([UserId]) REFERENCES [lnrpt].[User]([UserId])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanApplicationFact_Status] ON [lnrpt].[LoanApplicationFact] ([Status])

GO

CREATE NONCLUSTERED INDEX [IX_LoanApplicationFact_UserId] ON [lnrpt].[LoanApplicationFact] ([UserId])
