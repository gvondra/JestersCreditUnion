DROP PROCEDURE IF EXISTS `CreateTransaction`;
DELIMITER $$
CREATE PROCEDURE `CreateTransaction`(
OUT `id` CHAR(16),
`loanId` CHAR(16),
`date` DATE,
`type` SMALLINT,
`amount` DECIMAL(11, 2),
`paymentId` CHAR(16),
`termNumber` SMALLINT,
OUT `timestamp` TIMESTAMP 
)
BEGIN
	SET @id = UUID();
	SET @timestamp = UTC_TIMESTAMP(4);
	INSERT INTO `Transaction` (`TransactionId`, `LoanId`, `Date`, `Type`, `Amount`, `CreateTimestamp`)
	VALUES (@id, @loanId, @date, @type, @amount, @timestamp)
	;
	IF @paymentId IS NOT NULL AND @termNumber IS NOT NULL THEN
		INSERT INTO `PaymentTransaction` (`PaymentId`, `TransactionId`, `TermNumber`, `CreateTimestamp`)
		VALUES (@paymentId, @id, @termNumber, @timestamp);
	END IF;
END$$
DELIMITER ;
