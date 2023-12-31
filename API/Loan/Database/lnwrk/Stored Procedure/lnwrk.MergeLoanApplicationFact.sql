CREATE PROCEDURE [lnwrk].[MergeLoanApplicationFact]
AS
MERGE INTO [lnrpt].[LoanApplicationFact] as [tgt]
USING (SELECT [CreateTimestamp], [ApplicationDate], [ClosedDate], [Amount], [Status], [UserId]
	FROM [lnwrk].[LoanApplicationFact]) as [src] ([CreateTimestamp], [ApplicationDate], [ClosedDate], [Amount], [Status], [UserId])
ON [tgt].[CreateTimestamp] = [src].[CreateTimestamp]
	AND [tgt].[ApplicationDate] = [src].[ApplicationDate]
	AND (([tgt].[ClosedDate] is NULL AND [src].[ClosedDate] is NULL) or [tgt].[ClosedDate] = [src].[ClosedDate])
	AND [tgt].[Amount] = [src].[Amount]
	AND [tgt].[Status] = [src].[Status]
	AND (([tgt].[UserId] is NULL AND [src].[UserId] is NULL) or [tgt].[UserId] = [src].[UserId])
WHEN NOT MATCHED THEN
	INSERT ([CreateTimestamp], [ApplicationDate], [ClosedDate], [Amount], [Status], [UserId])
	VALUES ([src].[CreateTimestamp], [src].[ApplicationDate], [src].[ClosedDate], [src].[Amount], [src].[Status], [src].[UserId])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;