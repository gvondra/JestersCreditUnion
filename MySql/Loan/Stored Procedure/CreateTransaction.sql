DROP PROCEDURE IF EXISTS `CreateTransaction`;
DELIMITER $$
CREATE PROCEDURE `CreateTransaction`(
OUT `id` BINARY(16),
`loanId` BINARY(16),
`date` DATE,
`type` SMALLINT,
`amount` DECIMAL(11, 2),
`paymentId` BINARY(16),
`termNumber` SMALLINT,
OUT `timestamp` TIMESTAMP 
)
BEGIN
	SET `id` = UUID_TO_BIN(UUID());
	SET `timestamp` = UTC_TIMESTAMP(4);
	INSERT INTO `Transaction` (`TransactionId`, `LoanId`, `Date`, `Type`, `Amount`, `CreateTimestamp`)
	VALUES (`id`, `loanId`, `date`, `type`, `amount`, `timestamp`)
	;
	IF `paymentId` IS NOT NULL AND `termNumber` IS NOT NULL THEN
		INSERT INTO `PaymentTransaction` (`PaymentId`, `TransactionId`, `TermNumber`, `CreateTimestamp`)
		VALUES (`paymentId`, `id`, `termNumber`, `timestamp`);
	END IF;
END$$
DELIMITER ;

