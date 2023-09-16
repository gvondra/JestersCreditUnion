CREATE PROCEDURE [lnwrk].[MergeLoan]
AS
BEGIN
MERGE INTO [lnrpt].[Loan] as [tgt]
USING (SELECT [Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue]
	FROM [lnwrk].[Loan]) AS [src] ([Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue])
ON [tgt].[Number] = [src].[Number]
	and [tgt].[InitialDisbursementDate] = [src].[InitialDisbursementDate]
	and [tgt].[FirstPaymentDue] = [src].[FirstPaymentDue]
	and [tgt].[NextPaymentDue] = [src].[NextPaymentDue]
WHEN NOT MATCHED THEN
	INSERT ([Number], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue])
	VALUES ([src].[Number], [src].[InitialDisbursementDate], [src].[FirstPaymentDue], [src].[NextPaymentDue])
;
END