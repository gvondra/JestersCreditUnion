CREATE PROCEDURE [ln].[CreateLoan]
	@id UNIQUEIDENTIFIER OUT,
	@number VARCHAR(16),
	@loanApplicationId UNIQUEIDENTIFIER,
	@loanAgreementId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Loan] ([LoanId], [Number], [LoanApplicationId], [LoanAgreementId], [InitialDisbursementDate], 
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @number, @loanApplicationId, @loanAgreementId, @initialDisbursementDate,
	@timestamp, @timestamp);
END