CREATE PROCEDURE [ln].[GetTransaction_by_PaymentId]
	@paymentId UNIQUEIDENTIFIER
AS
SELECT [trn].[TransactionId], [trn].[LoanId], [trn].[Date], [trn].[Type], [trn].[Amount], [trn].[CreateTimestamp]
FROM [ln].[Transaction] [trn]
INNER JOIN [ln].[PaymentTransaction] [pt] on [trn].[TransactionId] = [pt].[TransactionId]
WHERE [pt].[PaymentId] = @paymentId
;