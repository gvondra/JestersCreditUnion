DROP PROCEDURE IF EXISTS `CreatePaymentIntake`;
DELIMITER $$
CREATE PROCEDURE `CreatePaymentIntake`(
OUT `id` CHAR(16),
`loanId` CHAR(16),
`paymentId` CHAR(16),
`transactionNumber` VARCHAR(128),
`date` DATE,
`amount` DECIMAL(8, 2),
`status` SMALLINT,
`userId` VARCHAR(64),
OUT `timestamp` TIMESTAMP
)
BEGIN
	SET @id = UUID();
	SET @timestamp = UTC_TIMESTAMP(4);
	INSERT INTO `PaymentIntake` (`PaymentIntakeId`,`LoanId`,`PaymentId`,`TransactionNumber`,`Date`,`Amount`,`Status`,
	`CreateTimestamp`,`UpdateTimestamp`,`CreateUserId`,`UpdateUserId`)
	VALUES (@id, @loanId, @paymentId, @transactionNumber, @date, @amount, @status,
	@timestamp, @timestamp, @userId, @userId);
END$$
DELIMITER ;
