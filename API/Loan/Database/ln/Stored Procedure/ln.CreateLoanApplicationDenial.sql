CREATE PROCEDURE [ln].[CreateLoanApplicationDenial]
	@id UNIQUEIDENTIFIER,
	@userId UNIQUEIDENTIFIER,
	@reason SMALLINT,
	@date DATE,
	@text NVARCHAR(MAX),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[LoanApplicationDenial] ([LoanApplicationId], [UserId], [Reason], [Date], [Text], [CreateTimestamp], [UpdateTimestamp])
	VALUES(@id, @userId, @reason, @date, @text, @timestamp, @timestamp);
END
