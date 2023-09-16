CREATE PROCEDURE [ln].[CreateLoanHistory]
	@id UNIQUEIDENTIFIER OUT,
	@loanId UNIQUEIDENTIFIER,
	@loanApplicationId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@status SMALLINT,
	@balance DECIMAL(11,2),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[LoanHistory] ([LoanHistoryId], [LoanId], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue], [Status], [Balance],
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @loanId, @loanApplicationId, @initialDisbursementDate, @firstPaymentDue, @nextPaymentDue, @status, @balance,
	@timestamp, @timestamp);
END