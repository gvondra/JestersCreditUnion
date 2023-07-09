CREATE PROCEDURE [ln].[CreateTransaction]
	@id UNIQUEIDENTIFIER OUT,
	@loanId UNIQUEIDENTIFIER,
	@date DATE,
	@type SMALLINT,
	@amount DECIMAL(7, 2),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Transaction] ([TransactionId], [LoanId], [Date], [Type], [Amount], [CreateTimestamp])
	VALUES (@id, @loanId, @date, @type, @amount, @timestamp)
	;
END