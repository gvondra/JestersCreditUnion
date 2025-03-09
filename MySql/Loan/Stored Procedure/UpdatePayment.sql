DROP PROCEDURE IF EXISTS `UpdatePayment`;
DELIMITER $$
CREATE PROCEDURE `UpdatePayment`(
`id` CHAR(16),
`amount` DECIMAL(8, 2),
`status` SMALLINT,
OUT `timestamp` TIMESTAMP
)
BEGIN
	SET `timestamp` = UTC_TIMESTAMP(4);
	UPDATE `Payment`
	SET `Amount` = `amount`,
	    `Status` = `status`,
	    `UpdateTimestamp` = `timestamp`
	WHERE `PaymentId` = `id`;
END$$
DELIMITER ;
