CREATE PROCEDURE [ln].[GetPayment_by_Date_LoanNumber_TransactionNumber]
	@date DATE,
	@loanNumber VARCHAR(128),
	@transactionNumber VARCHAR(128)
AS
BEGIN
SELECT [PaymentId],[LoanNumber],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [Date] = @date
  AND [LoanNumber] = @loanNumber
  AND [TransactionNumber] = @transactionNumber
ORDER BY [Date], [LoanNumber]
;
EXEC [ln].[GetTransaction_by_PaymentDate_LoanNumber_TransactionNumber] @date, @loanNumber, @transactionNumber
END