CREATE PROCEDURE [ln].[GetPaymentTransaction_by_PaymentStatus]
	@status SMALLINT
AS
SELECT [pt].[PaymentId], [trn].[TransactionId], [trn].[LoanId], [trn].[Date], [trn].[Type], [trn].[Amount], [trn].[CreateTimestamp]
FROM [ln].[Transaction] [trn]
INNER JOIN [ln].[PaymentTransaction] [pt] on [trn].[TransactionId] = [pt].[TransactionId]
WHERE EXISTS (SELECT TOP 1 1
  FROM [ln].[Payment] [pmt]
  WHERE [pmt].[PaymentId] = [pt].[PaymentId]
  AND [pmt].[Status] = @status);