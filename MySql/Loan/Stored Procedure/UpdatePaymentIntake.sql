DROP PROCEDURE IF EXISTS `UpdatePaymentIntake`;
DELIMITER $$
CREATE PROCEDURE `UpdatePaymentIntake`(
`id` CHAR(16),
`paymentId` CHAR(16),
`transactionNumber` VARCHAR(128),
`date` DATE,
`amount` DECIMAL(8, 2),
`status` SMALLINT,
`userId` VARCHAR(64),
OUT `timestamp` TIMESTAMP
)
BEGIN
	SET @timestamp = UTC_TIMESTAMP(4);
	UPDATE `PaymentIntake`
	SET `PaymentId` = @paymentId,
	`TransactionNumber` = @transactionNumber,
	`Date` = @date,
	`Amount` = @amount,
	`Status` = @status,
	`UpdateTimestamp` = @timestamp,
	`UpdateUserId` = @userId
	WHERE `PaymentIntakeId` = @id;
END$$
DELIMITER ;
