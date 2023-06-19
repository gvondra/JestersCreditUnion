CREATE PROCEDURE [ln].[UpdateLoanApplicationDenial]
	@id UNIQUEIDENTIFIER,
	@userId UNIQUEIDENTIFIER,
	@reason SMALLINT,
	@date DATE,
	@text NVARCHAR(MAX),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[LoanApplicationDenial]
	SET [UserId] = @userId, 
	[Reason] = @reason, 
	[Date] = @date, 
	[Text] = @text, 
	[UpdateTimestamp] = @timestamp
	WHERE [LoanApplicationId] = @id;
END
