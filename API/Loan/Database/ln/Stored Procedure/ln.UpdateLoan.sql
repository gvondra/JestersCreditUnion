﻿CREATE PROCEDURE [ln].[UpdateLoan]
	@id UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@status SMALLINT = 0,
	@balance DECIMAL(11,2) = NULL,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[Loan] 
	SET [InitialDisbursementDate] = @initialDisbursementDate, 
	[FirstPaymentDue] = @firstPaymentDue,
	[NextPaymentDue] = @nextPaymentDue,
	[Status] = @status,
	[Balance] = @balance,
	[UpdateTimestamp] = @timestamp
	WHERE [LoanId] = @id;

	DECLARE @historyId UNIQUEIDENTIFIER;
	DECLARE @historyTimestamp DATETIME2(4);
	EXEC [ln].[UpdateLoanHistory] @timestamp, @id, @initialDisbursementDate, @firstPaymentDue, @nextPaymentDue, @status, @balance, @historyTimestamp OUT;
	IF @@ROWCOUNT = 0
	BEGIN
		EXEC [ln].[CreateLoanHistory] @historyId OUT, @id, @initialDisbursementDate, @firstPaymentDue, @nextPaymentDue, @status, @balance, @historyTimestamp OUT;
	END
END