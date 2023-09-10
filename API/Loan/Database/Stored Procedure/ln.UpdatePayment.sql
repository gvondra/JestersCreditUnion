CREATE PROCEDURE [ln].[UpdatePayment]
	@id UNIQUEIDENTIFIER,
	@amount DECIMAL(8, 2),
	@status SMALLINT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[Payment]
	SET [Amount] = @amount,
	    [Status] = @status,
	    [UpdateTimestamp] = @timestamp
	WHERE [PaymentId] = @id;
END