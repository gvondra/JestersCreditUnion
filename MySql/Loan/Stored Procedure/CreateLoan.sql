DROP PROCEDURE IF EXISTS `CreateLoan`;
DELIMITER %%
CREATE PROCEDURE `CreateLoan`(
`id` BINARY(16),
`number` VARCHAR(16),
`loanApplicationId` BINARY(16),
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
	INSERT INTO `Loan` (`LoanId`, `Number`, `LoanApplicationId`, `InitialDisbursementDate`, `FirstPaymentDue`, `NextPaymentDue`, `Status`, `Balance`,
	`CreateTimestamp`, `UpdateTimestamp`)
	VALUES (`id`, `number`, `loanApplicationId`, `initialDisbursementDate`, `firstPaymentDue`, `nextPaymentDue`, `status`, `balance`,
	`timestamp`, `timestamp`);

	CALL `CreateLoanHistory`(`historyId`, `id`, `initialDisbursementDate`, `firstPaymentDue`, `nextPaymentDue`, `status`, `balance`, `historyTimestamp`);
END%%
DELIMITER ;