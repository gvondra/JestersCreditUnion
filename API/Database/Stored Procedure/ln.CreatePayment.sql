CREATE PROCEDURE [ln].[CreatePayment]
	@id UNIQUEIDENTIFIER,
	@loanNumber VARCHAR(128),
	@transactionNumber VARCHAR(128),
	@date DATE,
	@amount DECIMAL(8, 2),
	@status SMALLINT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Payment] ([PaymentId],[LoanNumber],[TransactionNumber],[Date],[Amount],[Status],
	[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @loanNumber, @transactionNumber, @date, @amount, @status,
	@timestamp, @timestamp);		
END