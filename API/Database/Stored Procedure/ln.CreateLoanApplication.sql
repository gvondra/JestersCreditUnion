﻿CREATE PROCEDURE [ln].[CreateLoanApplication]
	@id UNIQUEIDENTIFIER OUT,
	@userId UNIQUEIDENTIFIER,
	@status SMALLINT,
	@applicationDate DATE,
	@borrowerName NVARCHAR(1024),
	@borrowerBirthDate DATE,
	@borrowerAddressId UNIQUEIDENTIFIER,
	@borrowerEmailAddressId UNIQUEIDENTIFIER,
	@borrowerPhoneId UNIQUEIDENTIFIER,
	@borrowerEmployerName NVARCHAR(1024),
	@borrowerEmploymentHireDate DATE,
	@borrowerIncome DECIMAL(11, 2),
	@borrowerIdentificationCardName VARCHAR(32) = '',
	@coBorrowerName NVARCHAR(1024),
	@coBorrowerBirthDate DATE,
	@coBorrowerAddressId UNIQUEIDENTIFIER,
	@coBorrowerEmailAddressId UNIQUEIDENTIFIER,
	@coBorrowerPhoneId UNIQUEIDENTIFIER,
	@coBorrowerEmployerName NVARCHAR(1024),
	@coBorrowerEmploymentHireDate DATE,
	@coBorrowerIncome DECIMAL(11, 2),
	@amount DECIMAL(11, 2),
	@purpose NVARCHAR(2048),
	@mortgagePayment DECIMAL(7, 2),
	@rentPayment DECIMAL(7, 2),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[LoanApplication] (
	[LoanApplicationId], [UserId], [Status], [ApplicationDate], 
	[BorrowerName], [BorrowerBirthDate], [BorrowerAddressId], [BorrowerEmailAddressId], [BorrowerPhoneId], [BorrowerEmployerName], [BorrowerEmploymentHireDate], [BorrowerIncome],
	[BorrowerIdentificationCardName],
	[CoBorrowerName], [CoBorrowerBirthDate], [CoBorrowerAddressId], [CoBorrowerEmailAddressId], [CoBorrowerPhoneId], [CoBorrowerEmployerName], [CoBorrowerEmploymentHireDate], [CoBorrowerIncome],
	[Amount], [Purpose], [MortgagePayment], [RentPayment],
	[CreateTimestamp], [UpdateTimestamp])
	VALUES (
	@id, @userId, @status, @applicationDate,
	@borrowerName, @borrowerBirthDate, @borrowerAddressId, @borrowerEmailAddressId, @borrowerPhoneId, @borrowerEmployerName, @borrowerEmploymentHireDate, @borrowerIncome,
	@borrowerIdentificationCardName,
	@coBorrowerName, @coBorrowerBirthDate, @coBorrowerAddressId, @coBorrowerEmailAddressId, @coBorrowerPhoneId, @coBorrowerEmployerName, @coBorrowerEmploymentHireDate, @coBorrowerIncome,
	@amount, @purpose, @mortgagePayment, @rentPayment,
	@timestamp, @timestamp);
END
