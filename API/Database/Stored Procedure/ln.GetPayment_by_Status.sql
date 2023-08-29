CREATE PROCEDURE [ln].[GetPayment_by_Status]
	@status SMALLINT
AS
BEGIN
SELECT [PaymentId],[LoanId],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [Status] = @status
ORDER BY [Date]
;

EXEC [ln].[GetPaymentTransaction_by_PaymentStatus] @status;
END