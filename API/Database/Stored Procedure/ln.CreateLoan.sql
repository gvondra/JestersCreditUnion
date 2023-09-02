﻿CREATE PROCEDURE [ln].[CreateLoan]
	@id UNIQUEIDENTIFIER,
	@number VARCHAR(16),
	@loanApplicationId UNIQUEIDENTIFIER,
	@initialDisbursementDate DATE,
	@firstPaymentDue DATE,
	@nextPaymentDue DATE,
	@status SMALLINT = 0,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[Loan] ([LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue], [Status],
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @number, @loanApplicationId, @initialDisbursementDate, @firstPaymentDue, @nextPaymentDue, @status,
	@timestamp, @timestamp);
END