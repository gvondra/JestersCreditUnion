DROP PROCEDURE IF EXISTS `CreateLoanApplicationDenial`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanApplicationDenial`(
`id` CHAR(16),
`userId` CHAR(16),
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
