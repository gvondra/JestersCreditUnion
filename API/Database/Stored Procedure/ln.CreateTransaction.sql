CREATE PROCEDURE [ln].[CreateTransaction]
	@id UNIQUEIDENTIFIER OUT,
	@loanId UNIQUEIDENTIFIER,
	@date DATE,
	@type SMALLINT,
	@amount DECIMAL(7, 2),
	@paymentId UNIQUEIDENTIFIER = NULL,
	@termNumber SMALLINT = NULL,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Transaction] ([TransactionId], [LoanId], [Date], [Type], [Amount], [CreateTimestamp])
	VALUES (@id, @loanId, @date, @type, @amount, @timestamp)
	;
	IF @paymentId IS NOT NULL AND @termNumber IS NOT NULL
	BEGIN
		INSERT INTO [ln].[PaymentTransaction] ([PaymentId], [TransactionId], [TermNumber], [CreateTimestamp])
		VALUES (@paymentId, @id, @termNumber, @timestamp);
	END
END