CREATE PROCEDURE [ln].[GetLoanApplicationComment_by_UserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT [LoanApplicationCommentId], [LoanApplicationId], [UserId], [IsInternal], [Text], [CreateTimestamp]
FROM [ln].[LoanApplicationComment]
WHERE EXISTS (SELECT TOP 1 1 
	FROM [ln].[LoanApplication]
	WHERE [ln].[LoanApplication].[LoanApplicationId] = [ln].[LoanApplicationComment].[LoanApplicationId]
	AND [UserId] = @userId
)
ORDER BY [CreateTimestamp]
;