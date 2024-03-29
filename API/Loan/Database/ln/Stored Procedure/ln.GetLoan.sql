﻿CREATE PROCEDURE [ln].[GetLoan]
	@id UNIQUEIDENTIFIER
AS
BEGIN 
	SELECT TOP 1 [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue], [Status], [Balance],
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan]
	WHERE [LoanId] = @id;

	EXEC [ln].[GetLoanAgreement] @id;
END