CREATE PROCEDURE [jcu].[UpdateLoanApplication]
	@id UNIQUEIDENTIFIER,
	@status SMALLINT,
	@borrowerName VARCHAR(1012),
	@borrowerBirthDate DATE,
	@borrowerAddressId UNIQUEIDENTIFIER,
	@borrowerEmailAddressId UNIQUEIDENTIFIER,
	@borrowerPhoneId UNIQUEIDENTIFIER,
	@borrowerEmployerName VARCHAR(1024),
	@borrowerEmploymentHireDate DATE,
	@borrowerIncome DECIMAL(14,2),
	@coBorrowerName VARCHAR(1012),
	@coBorrowerBirthDate DATE,
	@coBorrowerAddressId UNIQUEIDENTIFIER,
	@coBorrowerEmailAddressId UNIQUEIDENTIFIER,
	@coBorrowerPhoneId UNIQUEIDENTIFIER,
	@coBorrowerEmployerName VARCHAR(1024),
	@coBorrowerEmploymentHireDate DATE,
	@coBorrowerIncome DECIMAL(14,2),
	@amount DECIMAL(11,2),
	@purpose NVARCHAR(MAX),
	@mortgagePayment DECIMAL(11,2),
	@rentPayment DECIMAL(11,2),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [jcu].[LoanApplication]
	SET [Status] = @status,
		[BorrowerName] = @borrowerName,
		[BorrowerBirthDate] = @borrowerBirthDate,
		[BorrowerAddressId] = @borrowerAddressId,
		[BorrowerEmailAddressId] = @borrowerEmailAddressId,
		[BorrowerPhoneId] = @borrowerPhoneId,
		[BorrowerEmployerName] = @borrowerEmployerName,
		[BorrowerEmploymentHireDate] = @borrowerEmploymentHireDate,
		[BorrowerIncome] = @borrowerIncome,

		[CoBorrowerName] = @coBorrowerName,
		[CoBorrowerBirthDate] = @coBorrowerBirthDate,
		[CoBorrowerAddressId] = @coBorrowerAddressId,
		[CoBorrowerEmailAddressId] = @coBorrowerEmailAddressId,
		[CoBorrowerPhoneId] = @coBorrowerPhoneId,
		[CoBorrowerEmployerName] = @coBorrowerEmployerName,
		[CoBorrowerEmploymentHireDate] = @coBorrowerEmploymentHireDate,
		[CoBorrowerIncome] = @coBorrowerIncome,
				
		[Amount] = @amount,
		[Purpose] = @purpose,
		[MortgagePayment] = @mortgagePayment,
		[RentPayment] = @rentPayment,
		[UpdateTimestamp] = @timestamp
	WHERE [LoanApplicationId] = @id
	;
END