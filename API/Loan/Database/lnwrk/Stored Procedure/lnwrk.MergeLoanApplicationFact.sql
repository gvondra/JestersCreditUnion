CREATE PROCEDURE [lnwrk].[MergeLoanApplicationFact]
AS
MERGE INTO [lnrpt].[LoanApplicationFact] as [tgt]
USING (SELECT [ApplicationDate], [ClosedDate], [Amount], [Status], [UserId], COUNT(1) [Count]
	FROM [lnwrk].[LoanApplicationFact]
	GROUP BY [ApplicationDate], [ClosedDate], [Amount], [Status], [UserId]) as [src] ([ApplicationDate], [ClosedDate], [Amount], [Status], [UserId], [Count])
ON [tgt].[ApplicationDate] = [src].[ApplicationDate]
	AND (([tgt].[ClosedDate] is NULL AND [src].[ClosedDate] is NULL) or [tgt].[ClosedDate] = [src].[ClosedDate])
	AND [tgt].[Amount] = [src].[Amount]
	AND [tgt].[Status] = [src].[Status]
	AND (([tgt].[UserId] is NULL AND [src].[UserId] is NULL) or [tgt].[UserId] = [src].[UserId])
WHEN NOT MATCHED THEN
	INSERT ([ApplicationDate], [ClosedDate], [Amount], [Status], [UserId], [Count])
	VALUES ([src].[ApplicationDate], [src].[ClosedDate], [src].[Amount], [src].[Status], [src].[UserId], [src].[Count])
WHEN MATCHED THEN
	UPDATE SET [Count] = [src].[Count]
WHEN NOT MATCHED BY SOURCE THEN DELETE
;