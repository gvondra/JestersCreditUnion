DROP PROCEDURE IF EXISTS `UpdateLoan`;
DELIMITER $$
CREATE PROCEDURE `UpdateLoan`(
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
	UPDATE `LoanAgreement` 
	SET 
	`Status` = `status`, 
	`CreateDate` = `createDate`, 
	`AgreementDate` = `agreementDate`,
	`BorrowerName` = `borrowerName`, 
	`BorrowerBirthDate` = `borrowerBirthDate`, 
	`BorrowerAddressId` = `borrowerAddressId`, 
	`BorrowerEmailAddressId` = `borrowerEmailAddressId`, 
	`BorrowerPhoneId` = `borrowerPhoneId`,
	`CoBorrowerName` = `coBorrowerName`, 
	`CoBorrowerBirthDate` = `coBorrowerBirthDate`, 
	`CoBorrowerAddressId` = `coBorrowerAddressId`, 
	`CoBorrowerEmailAddressId` = `coBorrowerEmailAddressId`, 
	`CoBorrowerPhoneId` = `coBorrowerPhoneId`,
	`OriginalAmount` = `originalAmount`, 
	`OriginalTerm` = `originalTerm`, 
	`InterestRate` = `interestRate`, 
	`PaymentAmount` = `paymentAmount`, 
	`PaymentFrequency` = `paymentFrequency`,
	`UpdateTimestamp` = `timestamp`
	WHERE `LoanId` = `id`;

	CALL `UpdateLoanAgreementHistory`(`timestamp`, `id`, `status`, `createDate`, `agreementDate`,
		`borrowerName`, `borrowerBirthDate`, `borrowerAddressId`, `borrowerEmailAddressId`, `borrowerPhoneId`,
		`coBorrowerName`, `coBorrowerBirthDate`, `coBorrowerAddressId`, `coBorrowerEmailAddressId`, `coBorrowerPhoneId`, 
		`originalAmount`, `originalTerm`, `interestRate`, `paymentAmount`, `paymentFrequency`, `historyTimestamp`);
	IF ROW_COUNT() = 0 THEN
		CALL `CreateLoanAgreementHistory`(`historyId`, `id`, `status`, `createDate`, `agreementDate`,
			`borrowerName`, `borrowerBirthDate`, `borrowerAddressId`, `borrowerEmailAddressId`, `borrowerPhoneId`,
			`coBorrowerName`, `coBorrowerBirthDate`, `coBorrowerAddressId`, `coBorrowerEmailAddressId`, `coBorrowerPhoneId`, 
			`originalAmount`, `originalTerm`, `interestRate`, `paymentAmount`, `paymentFrequency`, `historyTimestamp`);
	END IF;
END$$
DELIMITER ;
