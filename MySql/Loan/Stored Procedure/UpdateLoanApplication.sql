DROP PROCEDURE IF EXISTS `UpdateLoanApplication`;
DELIMITER $$
CREATE PROCEDURE `UpdateLoanApplication`(
`id` BINARY(16),
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
SET @timestamp = UTC_TIMESTAMP(4);
UPDATE `LoanApplication`
SET `Status` = @status, 
`ApplicationDate` = @applicationDate, 
`BorrowerName` = @borrowerName, 
`BorrowerBirthDate` = @borrowerBirthDate, 
`BorrowerAddressId` = @borrowerAddressId, 
`BorrowerEmailAddressId` = @borrowerEmailAddressId, 
`BorrowerPhoneId` = @borrowerPhoneId, 
`BorrowerEmployerName` = @borrowerEmployerName, 
`BorrowerEmploymentHireDate` = @borrowerEmploymentHireDate, 
`BorrowerIncome` = @borrowerIncome,
`BorrowerIdentificationCardId` = @borrowerIdentificationCardId,
`CoBorrowerName` = @coBorrowerName, 
`CoBorrowerBirthDate` = @coBorrowerBirthDate, 
`CoBorrowerAddressId` = @coBorrowerAddressId, 
`CoBorrowerEmailAddressId` = @coBorrowerEmailAddressId, 
`CoBorrowerPhoneId` = @coBorrowerPhoneId, 
`CoBorrowerEmployerName` = @coBorrowerEmployerName, 
`CoBorrowerEmploymentHireDate` = @coBorrowerEmploymentHireDate, 
`CoBorrowerIncome` = @coBorrowerIncome,
`Amount` = @amount, 
`Purpose` = @purpose, 
`MortgagePayment` = @mortgagePayment,
`RentPayment` = @rentPayment,
`ClosedDate` = @closedDate,
`UpdateTimestamp` = @timestamp
WHERE `LoanApplicationId` = @id;
END$$
DELIMITER ;
