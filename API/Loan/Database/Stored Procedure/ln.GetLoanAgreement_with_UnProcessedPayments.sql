CREATE PROCEDURE [ln].[GetLoanAgreement_with_UnprocessedPayments]
AS
SELECT TOP 1 [LoanId], [Status], [CreateDate], [AgreementDate],
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId],
	[OriginalAmount], [OriginalTerm], [InterestRate], [PaymentAmount], [PaymentFrequency],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanAgreement]
WHERE EXISTS (SELECT TOP 1 1
FROM [ln].[Payment]
WHERE [ln].[LoanAgreement].[LoanId] = [ln].[Payment].[LoanId]
AND [Status] = 0);