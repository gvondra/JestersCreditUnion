CREATE PROCEDURE [ln].[GetLoanAgreement]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 [LoanId], [Status], [CreateDate], [AgreementDate],
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId],
	[OriginalAmount], [OriginalTerm], [InterestRate], [PaymentAmount], [PaymentFrequency],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanAgreement]
WHERE [LoanId] = @id;