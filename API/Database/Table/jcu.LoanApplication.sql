CREATE TABLE [jcu].[LoanApplication]
(
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Status] SMALLINT CONSTRAINT [DF_LoanApplication_Status] DEFAULT (0) NOT NULL,
	[BorrowerName] VARCHAR(1012) NOT NULL,
	[BorrowerBirthDate] DATE NOT NULL,
	[BorrowerAddressId] UNIQUEIDENTIFIER NOT NULL,
	[BorrowerEmailAddressId] UNIQUEIDENTIFIER NOT NULL,
	[BorrowerPhoneId] UNIQUEIDENTIFIER NOT NULL,
	[BorrowerEmployerName] VARCHAR(1024) NOT NULL,
	[BorrowerEmploymentHireDate] DATE NULL,
	[BorrowerIncome] DECIMAL(14,2) NULL,
	[CoBorrowerName] VARCHAR(1012) NOT NULL,
	[CoBorrowerBirthDate] DATE NULL,
	[CoBorrowerAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerEmailAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerPhoneId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerEmployerName] VARCHAR(1024) NOT NULL,
	[CoBorrowerEmploymentHireDate] DATE NULL,
	[CoBorrowerIncome] DECIMAL(14,2) NULL,
	[Amount] DECIMAL(11,2) NOT NULL,
	[Purpose] NVARCHAR(MAX) NOT NULL,
	[MortgagePayment] DECIMAL(11,2) NULL,
	[RentPayment] DECIMAL(11,2) NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplication_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplication_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanApplication] PRIMARY KEY NONCLUSTERED ([LoanApplicationId]), 
    CONSTRAINT [FK_LoanApplication_Borrower_To_Address] FOREIGN KEY ([BorrowerAddressId]) REFERENCES [jcu].[Address]([AddressId]), 
    CONSTRAINT [FK_LoanApplication_Borrower_To_EmailAddress] FOREIGN KEY ([BorrowerEmailAddressId]) REFERENCES [jcu].[EmailAddress]([EmailAddressId]), 
    CONSTRAINT [FK_LoanApplication_Borrower_To_Phone] FOREIGN KEY ([BorrowerPhoneId]) REFERENCES [jcu].[Phone]([PhoneId]),
    CONSTRAINT [FK_LoanApplication_CoBorrower_To_Address] FOREIGN KEY ([CoBorrowerAddressId]) REFERENCES [jcu].[Address]([AddressId]), 
    CONSTRAINT [FK_LoanApplication_CoBorrower_To_EmailAddress] FOREIGN KEY ([CoBorrowerEmailAddressId]) REFERENCES [jcu].[EmailAddress]([EmailAddressId]), 
    CONSTRAINT [FK_LoanApplication_CoBorrower_To_Phone] FOREIGN KEY ([CoBorrowerPhoneId]) REFERENCES [jcu].[Phone]([PhoneId])
)

GO

CREATE INDEX [IX_LoanApplication_BorrowerAddressId] ON [jcu].[LoanApplication] ([BorrowerAddressId])

GO

CREATE INDEX [IX_LoanApplication_BorrowerEmailAddressId] ON [jcu].[LoanApplication] ([BorrowerAddressId])

GO

CREATE INDEX [IX_LoanApplication_BorrowerPhoneId] ON [jcu].[LoanApplication] ([BorrowerPhoneId])

GO

CREATE INDEX [IX_LoanApplication_CoBorrowerAddressId] ON [jcu].[LoanApplication] ([CoBorrowerAddressId])

GO

CREATE INDEX [IX_LoanApplication_CoBorrowerEmailAddressId] ON [jcu].[LoanApplication] ([CoBorrowerAddressId])

GO

CREATE INDEX [IX_LoanApplication_CoBorrowerPhoneId] ON [jcu].[LoanApplication] ([CoBorrowerPhoneId])

GO

CREATE INDEX [IX_LoanApplication_UserId] ON [jcu].[LoanApplication] ([UserId])
