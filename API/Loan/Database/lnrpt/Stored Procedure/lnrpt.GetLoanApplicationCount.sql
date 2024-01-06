CREATE PROCEDURE [lnrpt].[GetLoanApplicationCount]
	@minApplicationDate DATE
AS
SELECT DATEPART(YEAR, [f].[ApplicationDate]) [ApplicationYear], DATEPART(MONTH, [f].[ApplicationDate]) [ApplicationMonth],
SUM([f].[Count]) [Count]
FROM [lnrpt].[LoanApplicationFact] [f]
WHERE [f].[ApplicationDate] >= @minApplicationDate
GROUP BY DATEPART(YEAR, [f].[ApplicationDate]), DATEPART(MONTH, [f].[ApplicationDate])
;
