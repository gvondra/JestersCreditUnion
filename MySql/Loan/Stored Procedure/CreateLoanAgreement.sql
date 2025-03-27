DROP PROCEDURE IF EXISTS `CreateLoanAgreement`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanAgreement`(
`id` BINARY(16),
`status` SMALLINT,
`createDate` DATE,
`agreementDate` DATE,
`borrowerName` NVARCHAR(1024),
`borrowerBirthDate` DATE,
`borrowerAddressId` BINARY(16),
`borrowerEmailAddressId` BINARY(16),
`borrowerPhoneId` BINARY(16),
`coBorrowerName` NVARCHAR(1024),
`coBorrowerBirthDate` DATE,
`coBorrowerAddressId` BINARY(16),
`coBorrowerEmailAddressId` BINARY(16),
`coBorrowerPhoneId` BINARY(16),
`originalAmount` DECIMAL(11, 2),
`originalTerm` SMALLINT,
`interestRate` DECIMAL(5, 4),
`paymentAmount` DECIMAL(7, 2),
`paymentFrequency` SMALLINT,
OUT `timestamp` TIMESTAMP
)
BEGIN
    DECLARE `historyId` BINARY(16);
	DECLARE `historyTimestamp` TIMESTAMP;
	SET `timestamp` = UTC_TIMESTAMP(4);
	INSERT INTO `LoanAgreement` (
	`LoanId`, `Status`, `CreateDate`, `AgreementDate`,
	`BorrowerName`, `BorrowerBirthDate`, `BorrowerAddressId`, `BorrowerEmailAddressId`, `BorrowerPhoneId`,
	`CoBorrowerName`, `CoBorrowerBirthDate`, `CoBorrowerAddressId`, `CoBorrowerEmailAddressId`, `CoBorrowerPhoneId`,
	`OriginalAmount`, `OriginalTerm`, `InterestRate`, `PaymentAmount`, `PaymentFrequency`,
	`CreateTimestamp`, `UpdateTimestamp`) 
	VALUES (
	`id`, `status`, `createDate`, `agreementDate`,
	`borrowerName`, `borrowerBirthDate`, `borrowerAddressId`, `borrowerEmailAddressId`, `borrowerPhoneId`,
	`coBorrowerName`, `coBorrowerBirthDate`, `coBorrowerAddressId`, `coBorrowerEmailAddressId`, `coBorrowerPhoneId`,
	`originalAmount`, `originalTerm`, `interestRate`, `paymentAmount`, `paymentFrequency`,
	`timestamp`, `timestamp`);
	
	CALL `CreateLoanAgreementHistory`(`historyId`, `id`, `status`, `createDate`, `agreementDate`,
		`borrowerName`, `borrowerBirthDate`, `borrowerAddressId`, `borrowerEmailAddressId`, `borrowerPhoneId`,
		`coBorrowerName`, `coBorrowerBirthDate`, `coBorrowerAddressId`, `coBorrowerEmailAddressId`, `coBorrowerPhoneId`, 
		`originalAmount`, `originalTerm`, `interestRate`, `paymentAmount`, `paymentFrequency`, `historyTimestamp`);
END$$
DELIMITER ;
