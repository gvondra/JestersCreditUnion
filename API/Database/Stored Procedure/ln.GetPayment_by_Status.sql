CREATE PROCEDURE [ln].[GetPayment_by_Status]
	@status SMALLINT
AS
BEGIN
SELECT [PaymentId],[LoanNumber],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [Status] = @status
ORDER BY [Date], [LoanNumber]
;

EXEC [ln].[GetPaymentTransaction_by_PaymentStatus] @status;
END