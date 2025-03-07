DROP PROCEDURE IF EXISTS `CreateLoanApplicationRating`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanApplicationRating`(
OUT `id` CHAR(16),
`loanApplicationId` CHAR(16),
`value` DOUBLE,
OUT `timestamp` TIMESTAMP
)
BEGIN
    CALL `CreateRating`(@id, @value, @timestamp);

    UPDATE `LoanApplicationRating`
    SET `RatingId` = @id,
    `UpdateTimestamp` = @timestamp
    WHERE `LoanApplicationId` = @loanApplicationId;

    IF ROW_COUNT() = 0 THEN
        INSERT INTO `LoanApplicationRating` (`LoanApplicationId`, `RatingId`, `CreateTimestamp`, `UpdateTimestamp`)
        VALUES (@loanApplicationId, @id, @timestamp, @timestamp);
    ELSE 
        CALL `DeleteRating_Orphan`();
    END IF;
END$$
DELIMITER ;
