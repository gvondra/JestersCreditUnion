CREATE PROCEDURE [ln].[GetLoan_by_BorrowerNameBirthDate]
	@name NVARCHAR(1024),
	@birthDate DATE
AS
BEGIN
	SELECT [LoanId], [Number], [LoanApplicationId], [InitialDisbursementDate], [FirstPaymentDue], [NextPaymentDue], [Status],
		[CreateTimestamp], [UpdateTimestamp]
	FROM [ln].[Loan] [l]
	WHERE EXISTS (SELECT TOP 1 1
		FROM [ln].[LoanAgreement] [agr]
		WHERE [agr].[LoanId] = [l].[LoanId]
		AND (([agr].[BorrowerName] LIKE @name ESCAPE '\' AND [agr].[BorrowerBirthDate] = @birthDate)
		OR ([agr].[CoBorrowerName] LIKE @name ESCAPE '\' AND [agr].[CoBorrowerBirthDate] = @birthDate))
		);

	EXEC [ln].[GetLoanAgreement_by_BorrowerNameBirthDate] @name, @birthDate;
END