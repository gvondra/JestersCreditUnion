CREATE TABLE [ln].[LoanAgreement]
(
	[LoanId] UNIQUEIDENTIFIER NOT NULL,
	[Status] SMALLINT NOT NULL,
	[CreateDate] DATE NOT NULL,
	[AgreementDate] DATE NULL,
	[BorrowerName] NVARCHAR(1024) NOT NULL,
	[BorrowerBirthDate] DATE NOT NULL,
	[BorrowerAddressId] UNIQUEIDENTIFIER NULL,
	[BorrowerEmailAddressId] UNIQUEIDENTIFIER NULL,
	[BorrowerPhoneId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerName] NVARCHAR(1024) NOT NULL,
	[CoBorrowerBirthDate] DATE NULL,
	[CoBorrowerAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerEmailAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerPhoneId] UNIQUEIDENTIFIER NULL,
	[OriginalAmount] DECIMAL(11, 2) NOT NULL,
	[OriginalTerm] SMALLINT NOT NULL,
	[InterestRate] DECIMAL(5, 4) NOT NULL,
	[PaymentAmount] DECIMAL(7, 2) NOT NULL,
	[PaymentFrequency] SMALLINT NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanAgreement_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanAgreement_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanAgreement] PRIMARY KEY NONCLUSTERED ([LoanId])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanAgreement_BorrowerNameBirthDate] ON [ln].[LoanAgreement] ([BorrowerBirthDate] DESC, [BorrowerName])

GO

CREATE NONCLUSTERED INDEX [IX_LoanAgreement_CoBorrowerNameBirthDate] ON [ln].[LoanAgreement] ([CoBorrowerBirthDate] DESC, [CoBorrowerName])
WHERE [CoBorrowerName] <> '' AND [CoBorrowerBirthDate] IS NOT NULL
