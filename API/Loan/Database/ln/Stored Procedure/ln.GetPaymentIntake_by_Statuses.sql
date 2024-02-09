CREATE PROCEDURE [ln].[GetPaymentIntake_by_Statuses]
	@statues VARCHAR(512)
AS
SELECT [PaymentIntakeId],
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
INNER JOIN STRING_SPLIT(@statues, ',') [Ids] on [pIn].[Status] = CONVERT(SMALLINT, TRIM([Ids].[value]))
ORDER BY [Date], [LoanId]
;