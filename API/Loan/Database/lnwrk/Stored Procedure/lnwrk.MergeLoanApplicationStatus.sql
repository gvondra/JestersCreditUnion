CREATE PROCEDURE [lnwrk].[MergeLoanApplicationStatus]
AS
BEGIN
MERGE INTO [lnrpt].[LoanApplicationStatus] as [tgt]
USING (SELECT [Status], [Description] FROM [lnwrk].[LoanApplicationStatus]) as [src] ([Status], [Description])
ON [tgt].[Status] = [src].[Status]
WHEN NOT MATCHED THEN
	INSERT ([Status], [Description])
	VALUES ([src].[Status], [src].[Description])
WHEN MATCHED THEN
	UPDATE SET [Description] = [src].[Description]
;
END