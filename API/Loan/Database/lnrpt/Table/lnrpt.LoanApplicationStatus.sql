CREATE TABLE [lnrpt].[LoanApplicationStatus]
(
	[Status] SMALLINT NOT NULL,
	[Description] VARCHAR(1024) NOT NULL,
	CONSTRAINT [PK_LoanApplicationStatus] PRIMARY KEY NONCLUSTERED ([Status])
)
