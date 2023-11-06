CREATE TABLE [lnrpt].[LoanStatus]
(
	[Status] SMALLINT NOT NULL,
	[Description] VARCHAR(1024) NOT NULL,
	CONSTRAINT [PK_LoanStatus] PRIMARY KEY NONCLUSTERED ([Status])
)
