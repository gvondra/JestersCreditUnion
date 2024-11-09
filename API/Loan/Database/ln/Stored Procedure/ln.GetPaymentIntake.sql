CREATE PROCEDURE [ln].[GetPaymentIntake]
	@id UNIQUEIDENTIFIER
AS
SELECT [pIn].[PaymentIntakeId],
	[pIn].[LoanId],
	[pIn].[PaymentId],
	[pIn].[TransactionNumber],
	[pIn].[Date],
	[pIn].[Amount],
	[pIn].[Status],
	[pIn].[CreateTimestamp],
	[pIn].[UpdateTimestamp],
	[pIn].[CreateUserId],
	[pIn].[UpdateUserId]
FROM [ln].[PaymentIntake] [pIn]
WHERE [pIn].[PaymentIntakeId] = @id;