CREATE PROCEDURE [ln].[GetLoanApplication]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 
	[LoanApplicationId], [UserId], [Status], [ApplicationDate], 
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId], [BorrowerEmployerName], [BorrowerEmploymentHireDate], [BorrowerIncome],
	[BorrowerIdentificationCardId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId], [CoBorrowerEmployerName], [CoBorrowerEmploymentHireDate], [CoBorrowerIncome],
	[Amount], [Purpose], [MortgagePayment], [RentPayment],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanApplication]
WHERE [LoanApplicationId] = @id

EXEC [ln].[GetLoanApplicationDenial] @id;
EXEC [ln].[GetLoanApplicationComment_by_LoanApplicationId] @id;
EXEC [ln].[GetIdentificationCard_by_LoanApplicationId] @id;
;
