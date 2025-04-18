DROP PROCEDURE IF EXISTS `CreatePayment`;
DELIMITER $$
CREATE PROCEDURE `CreatePayment`(
`id` BINARY(16),
`loanId` BINARY(16),
`transactionNumber` VARCHAR(128),
`date` DATE,
`amount` DECIMAL(8, 2),
`status` SMALLINT,
OUT `timestamp` TIMESTAMP
)
BEGIN
	SET `timestamp` = UTC_TIMESTAMP(4);
	INSERT INTO `Payment` (`PaymentId`,`LoanId`,`TransactionNumber`,`Date`,`Amount`,`Status`,
	`CreateTimestamp`,`UpdateTimestamp`)
	VALUES (`id`, `loanId`, `transactionNumber`, `date`, `amount`, `status`,
	`timestamp`, `timestamp`);		
END$$
DELIMITER ;
