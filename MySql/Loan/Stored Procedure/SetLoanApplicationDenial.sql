DROP PROCEDURE IF EXISTS `SetLoanApplicationDenial`;
DELIMITER $$
CREATE PROCEDURE `SetLoanApplicationDenial`(
`id` CHAR(16),
`status` SMALLINT,
`closedDate` DATE,
`userId` CHAR(16),
`reason` SMALLINT,
`date` DATE,
`text` NVARCHAR(8000),
OUT `timestamp` TIMESTAMP
)
BEGIN
CALL `UpdateLoanApplicationDenial` (`id`, `userId`, `reason`, `date`, `text`, `timestamp`);

IF ROW_COUNT() = 0 THEN
    CALL `CreateLoanApplicationDenial` (`id`, `userId`, `reason`, `date`, `text`, `timestamp`);
END IF;

UPDATE `LoanApplication`
SET `Status` = `status`,
`ClosedDate` = `closedDate`,
`UpdateTimestamp` = `timestamp`
WHERE `LoanApplicationId` = `id`;
END$$
DELIMITER ;
