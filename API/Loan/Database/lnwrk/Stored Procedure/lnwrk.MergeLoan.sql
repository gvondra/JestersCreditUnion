CREATE PROCEDURE [lnwrk].[MergeLoan]
AS
BEGIN
MERGE INTO [lnrpt].[Loan] as [tgt]
USING (SELECT [Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue]
	FROM [lnwrk].[Loan]) AS [src] ([Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue])
ON [tgt].[Number] = [src].[Number]
	and (([tgt].[InitialDisbursementDate] is null and [src].[InitialDisbursementDate] is null) or [tgt].[InitialDisbursementDate] = [src].[InitialDisbursementDate])
	and (([tgt].[FirstPaymentDue] is null and [src].[FirstPaymentDue] is null) or [tgt].[FirstPaymentDue] = [src].[FirstPaymentDue])
	and (([tgt].[NextPaymentDue] is null and [src].[NextPaymentDue] is null) or [tgt].[NextPaymentDue] = [src].[NextPaymentDue])
WHEN NOT MATCHED THEN
	INSERT ([Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue])
	VALUES ([src].[Number], [src].[InitialDisbursementDate], [src].[FirstPaymentDue], [src].[NextPaymentDue])
;
END