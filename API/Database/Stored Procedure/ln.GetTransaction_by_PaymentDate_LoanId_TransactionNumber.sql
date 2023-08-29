CREATE PROCEDURE [ln].[GetTransaction_by_PaymentDate_LoanId_TransactionNumber]
	@date DATE,
	@loanId UNIQUEIDENTIFIER,
	@transactionNumber VARCHAR(128)
AS
SELECT [trn].[TransactionId], [trn].[LoanId], [trn].[Date], [trn].[Type], [trn].[Amount], [trn].[CreateTimestamp]
FROM [ln].[Transaction] [trn]
INNER JOIN [ln].[PaymentTransaction] [pt] on [trn].[TransactionId] = [pt].[TransactionId]
WHERE EXISTS (SELECT TOP 1 1
  FROM [ln].[Payment] [pmt]
  WHERE [pmt].[PaymentId] = [pt].[PaymentId]
  AND [pmt].[Date] = @date
  AND [pmt].[LoanId] = @loanId
  AND [pmt].[TransactionNumber] = @transactionNumber);