CREATE PROCEDURE [ln].[GetPayment_by_Date_LoanId_TransactionNumber]
	@date DATE,
	@loanId UNIQUEIDENTIFIER,
	@transactionNumber VARCHAR(128)
AS
BEGIN
SELECT [PaymentId],[LoanId],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [Date] = @date
  AND [LoanId] = @loanId
  AND [TransactionNumber] = @transactionNumber
ORDER BY [Date]
;
EXEC [ln].[GetPaymentTransaction_by_PaymentDate_LoanId_TransactionNumber] @date, @loanId, @transactionNumber
END