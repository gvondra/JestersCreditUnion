CREATE PROCEDURE [ln].[GetLoanAgreement_by_LoanNumber]
	@number VARCHAR(16)
AS
SELECT TOP 1 [LoanId], [Status], [CreateDate], [AgreementDate],
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId],
	[OriginalAmount], [OriginalTerm], [InterestRate], [PaymentAmount], [PaymentFrequency],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanAgreement]
WHERE EXISTS (SELECT TOP 1 1
	FROM [ln].[Loan]
	WHERE [ln].[Loan].[LoanId] = [ln].[LoanAgreement].[LoanId]
	AND [Number] = @number
);