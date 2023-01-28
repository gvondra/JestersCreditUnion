CREATE PROCEDURE [jcu].[GetLoanApplication_by_UserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT 
	[LoanApplicationId],
	[UserId],
	[Status],
	[BorrowerName],
	[BorrowerBirthDate],
	[BorrowerAddressId],
	[BorrowerEmailAddressId],
	[BorrowerPhoneId],
	[BorrowerEmployerName],
	[BorrowerEmploymentHireDate],
	[BorrowerIncome],
	[CoBorrowerName],
	[CoBorrowerBirthDate],
	[CoBorrowerAddressId],
	[CoBorrowerEmailAddressId],
	[CoBorrowerPhoneId],
	[CoBorrowerEmployerName],
	[CoBorrowerEmploymentHireDate],
	[CoBorrowerIncome],
	[Amount],
	[Purpose],
	[MortgagePayment],
	[RentPayment],
	[CreateTimestamp],
	[UpdateTimestamp]
FROM [jcu].[LoanApplication] 
WHERE [UserId] = @userId
ORDER BY [CreateTimestamp] DESC