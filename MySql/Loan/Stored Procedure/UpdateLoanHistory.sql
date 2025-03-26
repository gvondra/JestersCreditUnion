DROP PROCEDURE IF EXISTS `UpdateLoanHistory`;
DELIMITER $$
CREATE PROCEDURE `UpdateLoanHistory`(
`createTimestamp` TIMESTAMP,
`loanId` CHAR(16),
`initialDisbursementDate` DATE,
`firstPaymentDue` DATE,
`nextPaymentDue` DATE,
`status` SMALLINT,
`balance` DECIMAL(11,2),
OUT `timestamp` TIMESTAMP
)
BEGIN
	SET `timestamp` = UTC_TIMESTAMP(4);
	UPDATE `LoanHistory` 
	SET `InitialDisbursementDate` = `initialDisbursementDate`, 
	`FirstPaymentDue` = `firstPaymentDue`,
	`NextPaymentDue` = `nextPaymentDue`,
	`Status` = `status`,
	`Balance` = `balance`,
	`UpdateTimestamp` = `timestamp`
	WHERE `LoanHistory`.`CreateTimestamp` = `createTimestamp`
	AND `LoanHistory`.`LoanId` = `loanId`;
END$$
DELIMITER ;
