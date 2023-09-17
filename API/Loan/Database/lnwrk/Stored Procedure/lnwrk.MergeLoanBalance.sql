CREATE PROCEDURE [lnwrk].[MergeLoanBalance]
AS
BEGIN
MERGE INTO [lnrpt].[LoanBalance] as [tgt]
USING (SELECT [Date], [Balance], [LoanId], [LoanAgreementId], [LoanStatus]
	FROM [lnwrk].[LoanBalance]) as [src] ([Date], [Balance], [LoanId], [LoanAgreementId], [LoanStatus])
ON [tgt].[Date] = [src].[Date]
	and (([tgt].[Balance] is NULL and [src].[Balance] is NULL) or [tgt].[Balance] = [src].[Balance])
	and [tgt].[LoanId] = [src].[LoanId]
	and [tgt].[LoanAgreementId] = [src].[LoanAgreementId]
	and [tgt].[LoanStatus] = [src].[LoanStatus]
WHEN NOT MATCHED THEN
	INSERT ([Date], [Balance], [LoanId], [LoanAgreementId], [LoanStatus])
	VALUES ([src].[Date], [src].[Balance], [src].[LoanId], [src].[LoanAgreementId], [src].[LoanStatus])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
END