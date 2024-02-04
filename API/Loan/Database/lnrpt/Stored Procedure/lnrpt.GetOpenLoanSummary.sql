CREATE PROCEDURE [lnrpt].[GetOpenLoanSummary]
AS
SELECT [bal].[Balance], [ln].[NextPaymentDue], [ln].[Number]
FROM [lnrpt].[LoanBalance] [bal]
INNER JOIN [lnrpt].[Loan] [ln] on [bal].[LoanId] = [ln].[LoanId]
WHERE [bal].[LoanStatus] = 1 -- open
;