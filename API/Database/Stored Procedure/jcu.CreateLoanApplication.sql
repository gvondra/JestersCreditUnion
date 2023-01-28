CREATE PROCEDURE [jcu].[CreateLoanApplication]
	@id UNIQUEIDENTIFIER OUT,
	@userId UNIQUEIDENTIFIER,
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
	IF @id IS NULL SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [jcu].[LoanApplication] ([LoanApplicationId],[UserId],[Status],
		[BorrowerName],[BorrowerBirthDate],[BorrowerAddressId],[BorrowerEmailAddressId],[BorrowerPhoneId],[BorrowerEmployerName],[BorrowerEmploymentHireDate],[BorrowerIncome],
		[CoBorrowerName],[CoBorrowerBirthDate],[CoBorrowerAddressId],[CoBorrowerEmailAddressId],[CoBorrowerPhoneId],[CoBorrowerEmployerName],[CoBorrowerEmploymentHireDate],[CoBorrowerIncome],
		[Amount],[Purpose],[MortgagePayment],[RentPayment],
		[CreateTimestamp],[UpdateTimestamp]) 
	VALUES (@id, @userId, @status,
		@borrowerName, @borrowerBirthDate, @borrowerAddressId, @borrowerEmailAddressId, @borrowerPhoneId, @borrowerEmployerName, @borrowerEmploymentHireDate, @borrowerIncome,
		@coBorrowerName, @coBorrowerBirthDate, @coBorrowerAddressId, @coBorrowerEmailAddressId, @coBorrowerPhoneId, @coBorrowerEmployerName, @coBorrowerEmploymentHireDate, @coBorrowerIncome,
		@amount, @purpose, @mortgagePayment, @rentPayment,
		@timestamp, @timestamp)
	;
END