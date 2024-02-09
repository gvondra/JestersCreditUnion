CREATE PROCEDURE [ln].[CreatePaymentIntake]
	@id UNIQUEIDENTIFIER OUT,
	@loanId UNIQUEIDENTIFIER,
	@paymentId UNIQUEIDENTIFIER,
	@transactionNumber VARCHAR(128),
	@date DATE,
	@amount DECIMAL(8, 2),
	@status SMALLINT,
	@userId VARCHAR(64),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[PaymentIntake] ([PaymentIntakeId],[LoanId],[PaymentId],[TransactionNumber],[Date],[Amount],[Status],
	[CreateTimestamp],[UpdateTimestamp],[CreateUserId],[UpdateUserId])
	VALUES (@id, @loanId, @paymentId, @transactionNumber, @date, @amount, @status,
	@timestamp, @timestamp, @userId, @userId);
END