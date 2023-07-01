CREATE PROCEDURE [ln].[CreateLoan]
	@id UNIQUEIDENTIFIER,
	@number VARCHAR(16),
	@loanApplicationId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Loan] ([LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue],
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @number, @loanApplicationId, @initialDisbursementDate, @firstPaymentDue,
	@timestamp, @timestamp);
END