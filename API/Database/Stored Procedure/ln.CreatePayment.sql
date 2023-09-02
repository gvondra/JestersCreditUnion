CREATE PROCEDURE [ln].[CreatePayment]
	@id UNIQUEIDENTIFIER,
	@loanId UNIQUEIDENTIFIER,
	@transactionNumber VARCHAR(128),
	@date DATE,
	@amount DECIMAL(8, 2),
	@status SMALLINT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Payment] ([PaymentId],[LoanId],[TransactionNumber],[Date],[Amount],[Status],
	[CreateTimestamp],[UpdateTimestamp])
	VALUES (@id, @loanId, @transactionNumber, @date, @amount, @status,
	@timestamp, @timestamp);		
END