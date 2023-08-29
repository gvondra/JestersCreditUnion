CREATE PROCEDURE [ln].[GetTransaction_by_PaymentDate_LoanNumber_TransactionNumber]
	@date DATE,
	@loanNumber VARCHAR(128),
	@transactionNumber VARCHAR(128)
AS
SELECT [trn].[TransactionId], [trn].[LoanId], [trn].[Date], [trn].[Type], [trn].[Amount], [trn].[CreateTimestamp]
FROM [ln].[Transaction] [trn]
INNER JOIN [ln].[PaymentTransaction] [pt] on [trn].[TransactionId] = [pt].[TransactionId]
WHERE EXISTS (SELECT TOP 1 1
  FROM [ln].[Payment] [pmt]
  WHERE [pmt].[PaymentId] = [pt].[PaymentId]
  AND [pmt].[Date] = @date
  AND [pmt].[LoanNumber] = @loanNumber
  AND [pmt].[TransactionNumber] = @transactionNumber);