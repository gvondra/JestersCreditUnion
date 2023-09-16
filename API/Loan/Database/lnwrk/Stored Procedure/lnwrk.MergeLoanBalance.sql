CREATE PROCEDURE [lnwrk].[MergeLoanBalance]
AS
BEGIN
MERGE INTO [lnrpt].[LoanBalance] as [tgt]
USING (SELECT [Date], [Balance], [LoanId], [LoanAgreementId]
	FROM [lnwrk].[LoanBalance]) as [src] ([Date], [Balance], [LoanId], [LoanAgreementId])
ON [tgt].[Date] = [src].[Date]
	and [tgt].[Balance] = [src].[Balance]
	and [tgt].[LoanId] = [src].[LoanId]
	and [tgt].[LoanAgreementId] = [src].[LoanAgreementId]
WHEN NOT MATCHED THEN
	INSERT ([Date], [Balance], [LoanId], [LoanAgreementId])
	VALUES ([src].[Date], [src].[Balance], [src].[LoanId], [src].[LoanAgreementId])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
END