CREATE PROCEDURE [ln].[GetLoanApplication_by_UserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT
	[LoanApplicationId], [UserId], [Status], [ApplicationDate], 
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId], [BorrowerEmployerName], [BorrowerEmploymentHireDate], [BorrowerIncome],
	[BorrowerIdentificationCardId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId], [CoBorrowerEmployerName], [CoBorrowerEmploymentHireDate], [CoBorrowerIncome],
	[Amount], [Purpose], [MortgagePayment], [RentPayment],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanApplication]
WHERE [UserId] = @userId
ORDER BY [CreateTimestamp] DESC

EXEC [ln].[GetLoanApplicationDenial_by_UserId] @userId;
EXEC [ln].[GetLoanApplicationComment_by_UserId] @userId;
;
