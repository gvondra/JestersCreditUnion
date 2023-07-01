CREATE PROCEDURE [ln].[GetLoan_by_LoanApplicationId]
	@loanApplicationId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT TOP 1 [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], 
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan]
	WHERE [LoanApplicationId] = @loanApplicationId;

	EXEC [ln].[GetLoanAgreement_by_LoanApplicationId] @loanApplicationId;
END