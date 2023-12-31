CREATE TABLE [lnwrk].[LoanApplicationFact]
(
	[CreateTimestamp] DATETIME2(4) NOT NULL,
	[ApplicationDate] DATE NOT NULL,
	[ClosedDate] DATE NULL,
	[Amount] DECIMAL(11, 2) NOT NULL,
	[Status] SMALLINT NOT NULL,
	[UserId] UNIQUEIDENTIFIER NULL
)
