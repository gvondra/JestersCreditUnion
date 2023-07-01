CREATE PROCEDURE [ln].[GetLoan_by_Number]
	@number VARCHAR(16)
AS
BEGIN
	SELECT TOP 1 [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], 
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan]
	WHERE [Number] = @number;

	EXEC [ln].[GetLoanAgreement_by_LoanNumber] @number;
END