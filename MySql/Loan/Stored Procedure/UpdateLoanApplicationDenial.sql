DROP PROCEDURE IF EXISTS `UpdateLoanApplicationDenial`;
DELIMITER $$
CREATE DEFINER=`sa-dev-dataserver`@`%` PROCEDURE `UpdateLoanApplicationDenial`(
`id` BINARY(16),
`userId` BINARY(16),
`reason` SMALLINT,
`date` DATE,
`text` NVARCHAR(8000),
OUT `timestamp` TIMESTAMP
)
BEGIN
SET `timestamp` = UTC_TIMESTAMP(4);
UPDATE `LoanApplicationDenial`
SET `UserId` = `userId`,
    `Reason` = `reason`,
    `Date` = `date`,
    `Text` = `text`,
    `UpdateTimestamp` = `timestamp`
WHERE `LoanApplicationId` = `id`;
END$$
DELIMITER ;
