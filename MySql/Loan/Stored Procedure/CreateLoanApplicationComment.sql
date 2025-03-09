DROP PROCEDURE IF EXISTS `CreateLoanApplicationComment`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanApplicationComment`(
OUT `id` BINARY(16),
`loanApplicationId` BINARY(16),
`userId` BINARY(16),
`isInternal` BIT,
`text` NVARCHAR(8000),
OUT`timestamp` TIMESTAMP
)
BEGIN
	SET `id` = UUID_TO_BIN(UUID());
	SET `timestamp` = UTC_TIMESTAMP(4);
	INSERT INTO `LoanApplicationComment` (`LoanApplicationCommentId`, `LoanApplicationId`, `UserId`, `IsInternal`, `Text`, `CreateTimestamp`)
	VALUES (`id`, `loanApplicationId`, `userId`, `isInternal`, `text`, `timestamp`);
END$$
DELIMITER ;
