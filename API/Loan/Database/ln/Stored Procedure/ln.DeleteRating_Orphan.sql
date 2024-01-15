CREATE PROCEDURE [ln].[DeleteRating_Orphan]
AS
BEGIN
	DELETE FROM [ln].[RatingLog]
	WHERE NOT EXISTS (SELECT TOP 1 1
		FROM [ln].[LoanApplicationRating] [lar]
		WHERE [lar].[RatingId] = [ln].[RatingLog].[RatingId]);

	DELETE FROM [ln].[Rating]
	WHERE NOT EXISTS (SELECT TOP 1 1
		FROM [ln].[LoanApplicationRating] [lar]
		WHERE [lar].[RatingId] = [ln].[Rating].[RatingId])
	AND NOT EXISTS (SELECT TOP 1 1
		FROM [ln].[RatingLog] [rl]
		WHERE [rl].[RatingId] = [ln].[Rating].[RatingId]);
END