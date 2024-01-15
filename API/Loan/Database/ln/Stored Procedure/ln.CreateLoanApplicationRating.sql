CREATE PROCEDURE [ln].[CreateLoanApplicationRating]
	@id UNIQUEIDENTIFIER OUT,
	@loanApplicationId UNIQUEIDENTIFIER,
	@value REAL,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	EXEC [ln].[CreateRating] @id out, @value, @timestamp out;

	UPDATE [ln].[LoanApplicationRating]
	SET [RatingId] = @id,
	[UpdateTimestamp] = @timestamp
	WHERE [LoanApplicationId] = @loanApplicationId;

	IF (@@ROWCOUNT = 0)
	BEGIN
		INSERT INTO [ln].[LoanApplicationRating] ([LoanApplicationId], [RatingId], [CreateTimestamp], [UpdateTimestamp])
		VALUES (@loanApplicationId, @id, @timestamp, @timestamp);
	END
	ELSE
	BEGIN
		EXEC [ln].[DeleteRating_Orphan];
	END
END