CREATE TABLE [lnwrk].[LoanApplicationFact]
(
	[ApplicationDate] DATE NOT NULL,
	[ClosedDate] DATE NULL,
	[Amount] DECIMAL(11, 2) NOT NULL,
	[Status] SMALLINT NOT NULL,
	[UserId] UNIQUEIDENTIFIER NULL
)
