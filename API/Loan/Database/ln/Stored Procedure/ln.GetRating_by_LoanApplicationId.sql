CREATE PROCEDURE [ln].[GetRating_by_LoanApplicationId]
	@loanApplicationId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT [r].[RatingId], [r].[Value], [r].[CreateTimestamp]
	FROM [ln].[Rating] [r]
	INNER JOIN [ln].[LoanApplicationRating] [lar] on [r].[RatingId] = [lar].[RatingId]
	WHERE [lar].[LoanApplicationId] = @loanApplicationId
	;

	SELECT [rl].[RatingLogId], [rl].[RatingId], [rl].[Value], [rl].[Description], [rl].[CreateTimestamp]
	FROM [ln].[RatingLog] [rl]
	INNER JOIN [ln].[LoanApplicationRating] [lar] on [rl].[RatingId] = [lar].[RatingId]
	WHERE [lar].[LoanApplicationId] = @loanApplicationId
	;
END