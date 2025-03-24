DROP PROCEDURE IF EXISTS `CreateLoanApplicationDenial`;
DELIMITER $$
CREATE DEFINER=`sa-dev-dataserver`@`%` PROCEDURE `CreateLoanApplicationDenial`(
`id` BINARY(16),
`userId` BINARY(16),
`reason` SMALLINT,
`date` DATE,
`text` NVARCHAR(8000),
OUT `timestamp` TIMESTAMP
)
BEGIN
SET `timestamp` = UTC_TIMESTAMP(4);
INSERT INTO `LoanApplicationDenial` (`LoanApplicationId`, `UserId`, `Reason`, `Date`, `Text`, `CreateTimestamp`, `UpdateTimestamp`)
VALUES(`id`, `userId`, `reason`, `date`, `text`, `timestamp`, `timestamp`);
END$$
DELIMITER ;
