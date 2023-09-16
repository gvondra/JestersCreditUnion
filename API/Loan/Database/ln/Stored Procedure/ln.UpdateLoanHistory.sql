CREATE PROCEDURE [ln].[UpdateLoanHistory]
	@createTimestamp DATETIME2(4),
	@loanId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@status SMALLINT = 0,
	@balance DECIMAL(11,2) = NULL,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[LoanHistory] 
	SET [InitialDisbursementDate] = @initialDisbursementDate, 
	[FirstPaymentDue] = @firstPaymentDue,
	[NextPaymentDue] = @nextPaymentDue,
	[Status] = @status,
	[Balance] = @balance,
	[UpdateTimestamp] = @timestamp
	WHERE [CreateTimestamp] = @createTimestamp
	AND [LoanId] = @loanId;
END