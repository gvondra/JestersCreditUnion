﻿CREATE PROCEDURE [ln].[GetLoan_with_UnprocessedPayments]
AS
BEGIN 
	SELECT [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue],
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan]
	WHERE EXISTS (SELECT TOP 1 1
	FROM [ln].[Payment]
	WHERE [ln].[Loan].[LoanId] = [ln].[Payment].[LoanId]
	AND [Status] = 0);

	EXEC [ln].[GetLoanAgreement_with_UnprocessedPayments];
END