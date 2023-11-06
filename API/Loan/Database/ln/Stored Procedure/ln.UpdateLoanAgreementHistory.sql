CREATE PROCEDURE [ln].[UpdateLoanAgreementHistory]
	@createTimestamp DATETIME2(4),
	@loanId UNIQUEIDENTIFIER,
	@status SMALLINT,
	@createDate DATE,
	@agreementDate DATE,
	@borrowerName NVARCHAR(1024),
	@borrowerBirthDate DATE,
	@borrowerAddressId UNIQUEIDENTIFIER,
	@borrowerEmailAddressId UNIQUEIDENTIFIER,
	@borrowerPhoneId UNIQUEIDENTIFIER,
	@coBorrowerName NVARCHAR(1024),
	@coBorrowerBirthDate DATE,
	@coBorrowerAddressId UNIQUEIDENTIFIER,
	@coBorrowerEmailAddressId UNIQUEIDENTIFIER,
	@coBorrowerPhoneId UNIQUEIDENTIFIER,
	@originalAmount DECIMAL(11, 2),
	@originalTerm SMALLINT,
	@interestRate DECIMAL(5, 4),
	@paymentAmount DECIMAL(7, 2),
	@paymentFrequency SMALLINT,
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @timestamp = SYSUTCDATETIME();
	UPDATE [ln].[LoanAgreement] 
	SET 
	[Status] = @status, 
	[CreateDate] = @createDate, 
	[AgreementDate] = @agreementDate,
	[BorrowerName] = @borrowerName, 
	[BorrowerBirthDate] = @borrowerBirthDate, 
	[BorrowerAddressId] = @borrowerAddressId, 
	[BorrowerEmailAddressId] = @borrowerEmailAddressId, 
	[BorrowerPhoneId] = @borrowerPhoneId,
	[CoBorrowerName] = @coBorrowerName, 
	[CoBorrowerBirthDate] = @coBorrowerBirthDate, 
	[CoBorrowerAddressId] = @coBorrowerAddressId, 
	[CoBorrowerEmailAddressId] = @coBorrowerEmailAddressId, 
	[CoBorrowerPhoneId] = @coBorrowerPhoneId,
	[OriginalAmount] = @originalAmount, 
	[OriginalTerm] = @originalTerm, 
	[InterestRate] = @interestRate, 
	[PaymentAmount] = @paymentAmount, 
	[PaymentFrequency] = @paymentFrequency,
	[UpdateTimestamp] = @timestamp
	WHERE [CreateTimestamp] = @createTimestamp
	AND [LoanId] = @loanId;
END