CREATE PROCEDURE [ln].[UpdateLoan]
	@id UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[Loan] 
	SET [InitialDisbursementDate] = @initialDisbursementDate, 
	[UpdateTimestamp] = @timestamp
	WHERE [LoanId] = @id;
END