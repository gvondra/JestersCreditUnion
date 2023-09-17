CREATE PROCEDURE [lnwrk].[MergeLoanStatus]
AS
BEGIN
MERGE INTO [lnrpt].[LoanStatus] as [tgt]
USING (SELECT [Status], [Description] FROM [lnwrk].[LoanStatus]) as [src] ([Status], [Description])
ON [tgt].[Status] = [src].[Status]
WHEN NOT MATCHED THEN
	INSERT ([Status], [Description])
	VALUES ([src].[Status], [src].[Description])
WHEN MATCHED THEN
	UPDATE SET [Description] = [src].[Description]
;
END