DROP PROCEDURE IF EXISTS `UpdateLoanAgreementHistory`;
DELIMITER $$
CREATE PROCEDURE `UpdateLoanAgreementHistory`(
`createTimestamp` TIMESTAMP,
`loanId` BINARY(16),
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
	WHERE `LoanAgreement`.`CreateTimestamp` = `createTimestamp`
	AND `LoanAgreement`.`LoanId` = `loanId`;
END