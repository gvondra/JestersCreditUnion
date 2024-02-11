CREATE PROCEDURE [ln].[CommitPaymentIntake]
	@intakeStatusFilter SMALLINT,
	@intakeStatus SMALLINT,
	@paymentStatus SMALLINT,
	@userId VARCHAR(64)
AS
BEGIN
DECLARE intakeCursor CURSOR
FOR SELECT [PaymentIntakeId], [LoanId], [TransactionNumber], [Date], [Amount]
FROM [ln].[PaymentIntake]
WHERE [Status] = @intakeStatusFilter;

DECLARE @timestamp DATETIME2(4) = SYSUTCDATETIME();
DECLARE @paymentId UNIQUEIDENTIFIER;
DECLARE @paymentIntakeId UNIQUEIDENTIFIER;
DECLARE @loanId UNIQUEIDENTIFIER;
DECLARE @transactionNumber VARCHAR(128);
DECLARE @date DATE;
DECLARE @amount DECIMAL(8,2);

OPEN intakeCursor;
FETCH NEXT FROM intakeCursor INTO @paymentIntakeId, @loanId, @transactionNumber, @date, @amount;
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @paymentId = NEWID();
	INSERT INTO [ln].[Payment] ([PaymentId], [LoanId], [TransactionNumber], [Date], [Amount], [Status], [CreateTimestamp], [UpdateTimestamp])
	VALUES(@paymentId, @loanId, @transactionNumber, @date, @amount, @paymentStatus, @timestamp, @timestamp);

	UPDATE [ln].[PaymentIntake]
	SET [Status] = @intakeStatus,
	[PaymentId] = @paymentId,
	[UpdateTimestamp] = @timestamp,
	[UpdateUserId] = @userId
	WHERE [PaymentIntakeId] = @paymentIntakeId;

	FETCH NEXT FROM intakeCursor INTO @paymentIntakeId, @loanId, @transactionNumber, @date, @amount;
END
CLOSE intakeCursor;
DEALLOCATE intakeCursor;
	
END