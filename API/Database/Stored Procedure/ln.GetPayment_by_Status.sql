CREATE PROCEDURE [ln].[GetPayment_by_Status]
	@status SMALLINT
AS
SELECT [PaymentId],[LoanNumber],[TransactionNumber],[Date],[Amount],[Status],
  [CreateTimestamp],[UpdateTimestamp]
FROM [ln].[Payment]
WHERE [Status] = @status
ORDER BY [Date], [LoanNumber]
;