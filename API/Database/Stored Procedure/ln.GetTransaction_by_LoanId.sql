CREATE PROCEDURE [ln].[GetTransaction_by_LoanId]
	@loanId UNIQUEIDENTIFIER
AS
SELECT [TransactionId], [LoanId], [Date], [Type], [Amount], [CreateTimestamp]
FROM [ln].[Transaction]
ORDER BY [Date], [CreateTimestamp]
;