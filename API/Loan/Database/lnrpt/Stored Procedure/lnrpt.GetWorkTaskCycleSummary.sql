CREATE PROCEDURE [lnrpt].[GetWorkTaskCycleSummary]
	@minCreateDate DATE
AS
SELECT DATEPART(YEAR, [CreateDate]) [CreateYear], DATEPART(MONTH, [CreateDate]) [CreateMonth],
DATEPART(YEAR, [AssignedDate]) [AssignedYear], DATEPART(MONTH, [AssignedDate]) [AssignedMonth],
AVG(DATEDIFF(DAY, [CreateDate], [AssignedDate])) [DaysToAssigment],
DATEPART(YEAR, [ClosedDate]) [ClosedYear], DATEPART(MONTH, [ClosedDate]) [ClosedMonth],
AVG(DATEDIFF(DAY, [AssignedDate], [ClosedDate])) [DaysToClose],
AVG(DATEDIFF(DAY, [CreateDate], [ClosedDate])) [TotalDays],
[wtt].[Title], COUNT(1) [Count]

FROM [lnrpt].[WorkTaskCycle] [cyc]
INNER JOIN [lnrpt].[WorkTaskType] [wtt] on [cyc].[TypeCode] = [wtt].[Code]

WHERE [cyc].[CreateDate] >= @minCreateDate

GROUP BY DATEPART(YEAR, [CreateDate]), DATEPART(MONTH, [CreateDate]),
DATEPART(YEAR, [AssignedDate]), DATEPART(MONTH, [AssignedDate]),
DATEPART(YEAR, [ClosedDate]), DATEPART(MONTH, [ClosedDate]),
[wtt].[Title]

ORDER BY [wtt].[Title], DATEPART(YEAR, [CreateDate]), DATEPART(MONTH, [CreateDate]),
DATEPART(YEAR, [AssignedDate]), DATEPART(MONTH, [AssignedDate])
;