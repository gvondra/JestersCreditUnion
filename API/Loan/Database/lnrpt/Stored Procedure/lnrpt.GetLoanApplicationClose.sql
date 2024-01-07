CREATE PROCEDURE [lnrpt].[GetLoanApplicationClose]
	@minApplicationDate DATE
AS
SELECT DATEPART(YEAR, [f].[ClosedDate]) [ClosedYear], DATEPART(MONTH, [f].[ClosedDate]) [ClosedMonth],
[stat].[Description] [StatusDescription], SUM([f].[Count]) [Count]
FROM [lnrpt].[LoanApplicationFact] [f]
INNER JOIN [lnrpt].[LoanApplicationStatus] [stat] on [f].[Status] = [stat].[Status]
WHERE [f].[ApplicationDate] >= @minApplicationDate
AND [f].[ClosedDate] IS NOT NULL
GROUP BY DATEPART(YEAR, [f].[ClosedDate]), DATEPART(MONTH, [f].[ClosedDate]),
[stat].[Description]
;