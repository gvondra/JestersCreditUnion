CREATE PROCEDURE [ln].[UpdatePaymentIntake]
	@id UNIQUEIDENTIFIER,
	@paymentId UNIQUEIDENTIFIER,
	@transactionNumber VARCHAR(128),
	@date DATE,
	@amount DECIMAL(8, 2),
	@status SMALLINT,
	@userId VARCHAR(64),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[PaymentIntake]
	SET [PaymentId] = @paymentId,
	[TransactionNumber] = @transactionNumber,
	[Date] = @date,
	[Amount] = @amount,
	[Status] = @status,
	[UpdateTimestamp] = @timestamp,
	[UpdateUserId] = @userId
	WHERE [PaymentIntakeId] = @id;
END