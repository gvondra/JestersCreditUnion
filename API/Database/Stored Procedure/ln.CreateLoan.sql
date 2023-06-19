CREATE PROCEDURE [ln].[CreateLoan]
	@id UNIQUEIDENTIFIER OUT,
	@number VARCHAR(16),
	@loanApplicationId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Loan] ([LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], 
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @number, @loanApplicationId, @initialDisbursementDate,
	@timestamp, @timestamp);
END