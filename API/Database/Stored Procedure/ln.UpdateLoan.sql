CREATE PROCEDURE [ln].[UpdateLoan]
	@id UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[Loan] 
	SET [InitialDisbursementDate] = @initialDisbursementDate, 
	[FirstPaymentDue] = @firstPaymentDue,
	[NextPaymentDue] = @nextPaymentDue,
	[UpdateTimestamp] = @timestamp
	WHERE [LoanId] = @id;
END