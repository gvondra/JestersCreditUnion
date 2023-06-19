CREATE PROCEDURE [ln].[GetLoanApplicationDenial]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 
	[LoanApplicationId], [UserId], [Reason], [Date], [Text], [CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanApplicationDenial]
WHERE [LoanApplicationId] = @id
;