CREATE PROCEDURE [ln].[UpdateLoan]
	@id UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@status SMALLINT = 0,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[Loan] 
	SET [InitialDisbursementDate] = @initialDisbursementDate, 
	[FirstPaymentDue] = @firstPaymentDue,
	[NextPaymentDue] = @nextPaymentDue,
	[Status] = @status,
	[UpdateTimestamp] = @timestamp
	WHERE [LoanId] = @id;
END