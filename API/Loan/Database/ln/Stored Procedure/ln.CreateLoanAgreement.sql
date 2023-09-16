CREATE PROCEDURE [ln].[CreateLoanAgreement]
	@id UNIQUEIDENTIFIER,
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
	INSERT INTO [ln].[LoanAgreement] (
	[LoanId], [Status], [CreateDate], [AgreementDate],
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId],
	[OriginalAmount], [OriginalTerm], [InterestRate], [PaymentAmount], [PaymentFrequency],
	[CreateTimestamp], [UpdateTimestamp]) 
	VALUES (
	@id, @status, @createDate, @agreementDate,
	@borrowerName, @borrowerBirthDate, @borrowerAddressId, @borrowerEmailAddressId, @borrowerPhoneId,
	@coBorrowerName, @coBorrowerBirthDate, @coBorrowerAddressId, @coBorrowerEmailAddressId, @coBorrowerPhoneId,
	@originalAmount, @originalTerm, @interestRate, @paymentAmount, @paymentFrequency,
	@timestamp, @timestamp);
END