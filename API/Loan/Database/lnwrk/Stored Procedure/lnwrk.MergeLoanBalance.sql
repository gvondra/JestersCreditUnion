CREATE PROCEDURE [lnwrk].[MergeLoanBalance]
AS
BEGIN
MERGE INTO [lnrpt].[LoanBalance] as [tgt]
USING (SELECT [Date], [Balance], [LoanStatus],

	(SELECT MIN([LoanId]) FROM [lnrpt].[Loan] [ln]
	WHERE [ln].[Number] = [bal].[Number]
	AND (([ln].[InitialDisbursementDate] is NULL and [bal].[InitialDisbursementDate] is NULL) or [ln].[InitialDisbursementDate] = [bal].[InitialDisbursementDate])
	AND (([ln].[FirstPaymentDue] is NULL and [bal].[FirstPaymentDue] is NULL) or [ln].[FirstPaymentDue] = [bal].[FirstPaymentDue])
	AND (([ln].[NextPaymentDue] is NULL and [bal].[NextPaymentDue] is NULL) or [ln].[NextPaymentDue] = [bal].[NextPaymentDue])) [LoanId],

	(SELECT MIN([LoanAgreementId]) FROM [lnrpt].[LoanAgreement] [la]
	WHERE [la].[Hash] = [bal].[AgreementHash]
	AND [la].[CreateDate] = [bal].[AgreementCreateDate]
	AND (([la].[AgreementDate] is NULL and [bal].[AgreementDate] is NULL) or [la].[AgreementDate] = [bal].[AgreementDate])
	AND [la].[InterestRate] = [bal].[InterestRate]
	AND [la].[PaymentAmount] = [bal].[PaymentAmount]) [LoanAgreementId]

	FROM [lnwrk].[LoanBalance] [bal]
	WHERE [bal].[Balance] IS NOT NULL
	AND [bal].[Timestamp] = (SELECT MAX([Timestamp]) FROM [lnwrk].[LoanBalance]
		WHERE [Number] = [bal].[Number] and [Date] = [bal].[Date])) as [src] ([Date], [Balance], [LoanStatus], [LoanId], [LoanAgreementId])
ON [tgt].[Date] = [src].[Date]
	and (([tgt].[Balance] is NULL and [src].[Balance] is NULL) or [tgt].[Balance] = [src].[Balance])
	and [tgt].[LoanId] = [src].[LoanId]
	and [tgt].[LoanAgreementId] = [src].[LoanAgreementId]
	and [tgt].[LoanStatus] = [src].[LoanStatus]
WHEN NOT MATCHED THEN
	INSERT ([Date], [Balance], [LoanId], [LoanAgreementId], [LoanStatus])
	VALUES ([src].[Date], [src].[Balance], [src].[LoanId], [LoanAgreementId], [src].[LoanStatus])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
END