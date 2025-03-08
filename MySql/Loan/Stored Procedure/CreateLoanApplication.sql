DROP PROCEDURE IF EXISTS `CreateLoanApplication`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanApplication`(
OUT `id` BINARY(16),
`userId` BINARY(16),
`status` SMALLINT,
`applicationDate` DATE,
`borrowerName` NVARCHAR(1024),
`borrowerBirthDate` DATE,
`borrowerAddressId` BINARY(16),
`borrowerEmailAddressId` BINARY(16),
`borrowerPhoneId` BINARY(16),
`borrowerEmployerName` NVARCHAR(1024),
`borrowerEmploymentHireDate` DATE,
`borrowerIncome` DECIMAL(11, 2),
`borrowerIdentificationCardId` BINARY(16),
`coBorrowerName` NVARCHAR(1024),
`coBorrowerBirthDate` DATE,
`coBorrowerAddressId` BINARY(16),
`coBorrowerEmailAddressId` BINARY(16),
`coBorrowerPhoneId` BINARY(16),
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
SET `id` = UUID_TO_BIN(UUID());
SET `timestamp` = UTC_TIMESTAMP(4);
INSERT INTO `LoanApplication` (
    `LoanApplicationId`, `UserId`, `Status`, `ApplicationDate`, 
    `BorrowerName`, `BorrowerBirthDate`, `BorrowerAddressId`, `BorrowerEmailAddressId`, `BorrowerPhoneId`, `BorrowerEmployerName`, `BorrowerEmploymentHireDate`, `BorrowerIncome`,
    `BorrowerIdentificationCardId`,
    `CoBorrowerName`, `CoBorrowerBirthDate`, `CoBorrowerAddressId`, `CoBorrowerEmailAddressId`, `CoBorrowerPhoneId`, `CoBorrowerEmployerName`, `CoBorrowerEmploymentHireDate`, `CoBorrowerIncome`,
    `Amount`, `Purpose`, `MortgagePayment`, `RentPayment`,`ClosedDate`,
    `CreateTimestamp`, `UpdateTimestamp`)
VALUES (
    `id`, `userId`, `status`, `applicationDate`,
    `borrowerName`, `borrowerBirthDate`, `borrowerAddressId`, `borrowerEmailAddressId`, `borrowerPhoneId`, `borrowerEmployerName`, `borrowerEmploymentHireDate`, `borrowerIncome`,
    `borrowerIdentificationCardId`,
    `coBorrowerName`, `coBorrowerBirthDate`, `coBorrowerAddressId`, `coBorrowerEmailAddressId`, `coBorrowerPhoneId`, `coBorrowerEmployerName`, `coBorrowerEmploymentHireDate`, `coBorrowerIncome`,
    `amount`, `purpose`, `mortgagePayment`, `rentPayment`, `closedDate`,
    `timestamp`, `timestamp`);
END$$
DELIMITER ;
