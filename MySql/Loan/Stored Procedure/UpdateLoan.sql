DROP PROCEDURE IF EXISTS `UpdateLoan`;
DELIMITER $$
CREATE DEFINER=`sa-dev-dataserver`@`%` PROCEDURE `UpdateLoan`(
`id` BINARY(16),
`initialDisbursementDate` DATE,
`firstPaymentDue` DATE,
`nextPaymentDue` DATE,
`status` SMALLINT,
`balance` DECIMAL(11,2),
OUT `timestamp` TIMESTAMP
)
BEGIN
	DECLARE `historyId` BINARY(16);
	DECLARE `historyTimestamp` TIMESTAMP;
	SET `timestamp` = UTC_TIMESTAMP(4);
	UPDATE `Loan` 
	SET `InitialDisbursementDate` = `initialDisbursementDate`, 
	`FirstPaymentDue` = `firstPaymentDue`,
	`NextPaymentDue` = `nextPaymentDue`,
	`Status` = `status`,
	`Balance` = `balance`,
	`UpdateTimestamp` = `timestamp`
	WHERE `LoanId` = `id`;

	CALL `UpdateLoanHistory`(`timestamp`, `id`, `initialDisbursementDate`, `firstPaymentDue`, `nextPaymentDue`, `status`, `balance`, `historyTimestamp`);
	IF ROW_COUNT() = 0 THEN
		CALL `CreateLoanHistory`(`historyId`, `id`, `initialDisbursementDate`, `firstPaymentDue`, `nextPaymentDue`, `status`, `balance`, `historyTimestamp`);
	END IF;
END$$
DELIMITER ;
