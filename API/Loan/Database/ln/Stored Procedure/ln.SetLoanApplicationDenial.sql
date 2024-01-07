CREATE PROCEDURE [ln].[SetLoanApplicationDenial]
	@id UNIQUEIDENTIFIER,
	@status SMALLINT,
	@closedDate DATE,
	@userId UNIQUEIDENTIFIER,
	@reason SMALLINT,
	@date DATE,
	@text NVARCHAR(MAX),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	EXEC [ln].[UpdateLoanApplicationDenial] @id, @userId, @reason, @date, @text, @timestamp OUT;

	IF (@@ROWCOUNT = 0)
	BEGIN
		EXEC [ln].[CreateLoanApplicationDenial] @id, @userId, @reason, @date, @text, @timestamp OUT;
	END

	UPDATE [ln].[LoanApplication]
	SET [Status] = @status,
	[ClosedDate] = @closedDate,
	[UpdateTimestamp] = @timestamp
	WHERE [LoanApplicationId] = @id;
END