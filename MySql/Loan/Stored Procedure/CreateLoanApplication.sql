DROP PROCEDURE IF EXISTS `CreateLoanApplication`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanApplication`(
OUT `id` CHAR(16),
`userId` CHAR(16),
`status` SMALLINT,
`applicationDate` DATE,
`borrowerName` NVARCHAR(1024),
`borrowerBirthDate` DATE,
`borrowerAddressId` CHAR(16),
`borrowerEmailAddressId` CHAR(16),
`borrowerPhoneId` CHAR(16),
`borrowerEmployerName` NVARCHAR(1024),
`borrowerEmploymentHireDate` DATE,
`borrowerIncome` DECIMAL(11, 2),
`borrowerIdentificationCardId` CHAR(16),
`coBorrowerName` NVARCHAR(1024),
`coBorrowerBirthDate` DATE,
`coBorrowerAddressId` CHAR(16),
`coBorrowerEmailAddressId` CHAR(16),
`coBorrowerPhoneId` CHAR(16),
`coBorrowerEmployerName` NVARCHAR(1024),
`coBorrowerEmploymentHireDate` DATE,
`coBorrowerIncome` DECIMAL(11, 2),
`amount` DECIMAL(11, 2),
`purpose` NVARCHAR(2048),
`mortgagePayment` DECIMAL(7, 2),
`rentPayment` DECIMAL(7, 2),
`closedDate` DATE,
OUT `timestamp` TIMESTAMP
)
BEGIN
SET @id = UUID();
SET @timestamp = UTC_TIMESTAMP(4);
INSERT INTO `LoanApplication` (
    `LoanApplicationId`, `UserId`, `Status`, `ApplicationDate`, 
    `BorrowerName`, `BorrowerBirthDate`, `BorrowerAddressId`, `BorrowerEmailAddressId`, `BorrowerPhoneId`, `BorrowerEmployerName`, `BorrowerEmploymentHireDate`, `BorrowerIncome`,
    `BorrowerIdentificationCardId`,
    `CoBorrowerName`, `CoBorrowerBirthDate`, `CoBorrowerAddressId`, `CoBorrowerEmailAddressId`, `CoBorrowerPhoneId`, `CoBorrowerEmployerName`, `CoBorrowerEmploymentHireDate`, `CoBorrowerIncome`,
    `Amount`, `Purpose`, `MortgagePayment`, `RentPayment`,`ClosedDate`,
    `CreateTimestamp`, `UpdateTimestamp`)
VALUES (
    @id, @userId, @status, @applicationDate,
    @borrowerName, @borrowerBirthDate, @borrowerAddressId, @borrowerEmailAddressId, @borrowerPhoneId, @borrowerEmployerName, @borrowerEmploymentHireDate, @borrowerIncome,
    @borrowerIdentificationCardId,
    @coBorrowerName, @coBorrowerBirthDate, @coBorrowerAddressId, @coBorrowerEmailAddressId, @coBorrowerPhoneId, @coBorrowerEmployerName, @coBorrowerEmploymentHireDate, @coBorrowerIncome,
    @amount, @purpose, @mortgagePayment, @rentPayment,@closedDate,
    @timestamp, @timestamp);
END$$
DELIMITER ;
