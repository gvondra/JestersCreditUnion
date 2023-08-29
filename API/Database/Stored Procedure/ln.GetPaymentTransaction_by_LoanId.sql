CREATE PROCEDURE [ln].[GetPaymentTransaction_by_LoanId]
	@loanId UNIQUEIDENTIFIER
AS
SELECT [pt].[PaymentId], [pt].[TermNumber], [trn].[TransactionId], [trn].[LoanId], [trn].[Date], [trn].[Type], [trn].[Amount], [trn].[CreateTimestamp]
FROM [ln].[Transaction] [trn]
INNER JOIN [ln].[PaymentTransaction] [pt] on [trn].[TransactionId] = [pt].[TransactionId]
WHERE EXISTS (SELECT TOP 1 1
  FROM [ln].[Payment] [pmt]
  WHERE [pmt].[PaymentId] = [pt].[PaymentId]
  AND [pmt].[LoanId] = @loanId);