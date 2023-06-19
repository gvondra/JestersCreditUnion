CREATE PROCEDURE [ln].[GetLoanApplicationDenial_by_UserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT 
	[LoanApplicationId], [UserId], [Reason], [Date], [Text], [CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanApplicationDenial]
WHERE EXISTS (SELECT TOP 1 1 
	FROM [ln].[LoanApplication]
	WHERE [ln].[LoanApplication].[LoanApplicationId] = [ln].[LoanApplicationDenial].[LoanApplicationId]
	AND [UserId] = @userId
);