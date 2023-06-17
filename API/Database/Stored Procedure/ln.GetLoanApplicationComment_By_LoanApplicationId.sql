CREATE PROCEDURE [ln].[GetLoanApplicationComment_by_LoanApplicationId]
	@loanApplicationId UNIQUEIDENTIFIER
AS
SELECT [LoanApplicationCommentId], [LoanApplicationId], [UserId], [IsInternal], [Text], [CreateTimestamp]
FROM [ln].[LoanApplicationComment]
WHERE [LoanApplicationId] = @loanApplicationId
ORDER BY [CreateTimestamp]
;