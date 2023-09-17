CREATE PROCEDURE [ln].[GetLoanAgreement_by_BorrowerNameBirthDate]
	@name NVARCHAR(1024),
	@birthDate DATE
AS
SELECT [LoanId], [Status], [CreateDate], [AgreementDate],
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId],
	[OriginalAmount], [OriginalTerm], [InterestRate], [PaymentAmount], [PaymentFrequency],
	[CreateTimestamp], [UpdateTimestamp]
FROM [ln].[LoanAgreement]
WHERE ([BorrowerName] LIKE @name ESCAPE '\' AND [BorrowerBirthDate] = @birthDate)
	OR ([CoBorrowerName] LIKE @name ESCAPE '\' AND [CoBorrowerBirthDate] = @birthDate)
;