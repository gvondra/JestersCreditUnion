CREATE PROCEDURE [ln].[GetPayment_by_LoanId]
	@loanId UNIQUEIDENTIFIER
AS
BEGIN
SELECT [PaymentId],[LoanId],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [LoanId] = @loanId
ORDER BY [Date]
;

EXEC [ln].[GetPaymentTransaction_by_LoanId] @loanId;
END