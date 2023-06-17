CREATE TABLE [ln].[LoanApplication]
(
	[LoanApplicationId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Status] SMALLINT NOT NULL,
	[ApplicationDate] DATE NOT NULL,
	[BorrowerName] NVARCHAR(1024) NOT NULL,
	[BorrowerBirthDate] DATE NOT NULL,
	[BorrowerAddressId] UNIQUEIDENTIFIER NULL,
	[BorrowerEmailAddressId] UNIQUEIDENTIFIER NULL,
	[BorrowerPhoneId] UNIQUEIDENTIFIER NULL,
	[BorrowerEmployerName] NVARCHAR(1024) NOT NULL,
	[BorrowerEmploymentHireDate] DATE NULL,
	[BorrowerIncome] DECIMAL(11, 2) NULL,
	[CoBorrowerName] NVARCHAR(1024) NOT NULL,
	[CoBorrowerBirthDate] DATE NULL,
	[CoBorrowerAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerEmailAddressId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerPhoneId] UNIQUEIDENTIFIER NULL,
	[CoBorrowerEmployerName] NVARCHAR(1024) NOT NULL,
	[CoBorrowerEmploymentHireDate] DATE NULL,
	[CoBorrowerIncome] DECIMAL(11, 2) NULL,
	[Amount] DECIMAL(11, 2) NOT NULL,
	[Purpose] NVARCHAR(2048) NOT NULL,
	[MortgagePayment] DECIMAL(7, 2) NOT NULL,
	[RentPayment] DECIMAL(7, 2) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplication_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_LoanApplication_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_LoanApplication] PRIMARY KEY NONCLUSTERED ([LoanApplicationId]), 
    CONSTRAINT [FK_LoanApplication_To_LoanApplicationDenial] FOREIGN KEY ([LoanApplicationId]) REFERENCES [ln].[LoanApplicationDenial]([LoanApplicationId])
)

GO

CREATE NONCLUSTERED INDEX [IX_LoanApplication_UserId] ON [ln].[LoanApplication] ([UserId])
