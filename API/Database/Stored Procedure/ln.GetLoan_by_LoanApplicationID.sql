CREATE PROCEDURE [ln].[GetLoan_by_LoanApplicatinId]
	@loanApplicationId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT TOP 1 [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], 
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan]
	WHERE [LoanApplicationId] = @loanApplicationId;

	EXEC [ln].[GetLoanAgreement_by_LoanApplicationId] @loanApplicationId;
END