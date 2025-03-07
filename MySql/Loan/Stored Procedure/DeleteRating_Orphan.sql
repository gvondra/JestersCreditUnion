DROP PROCEDURE IF EXISTS `DeleteRating_Orphan`;
DELIMITER $$
CREATE PROCEDURE `DeleteRating_Orphan`()
BEGIN
DELETE FROM `RatingLog`
WHERE NOT EXISTS (SELECT 1
    FROM `LoanApplicationRating` `lar`
    WHERE `lar`.`RatingId` = `RatingLog`.`RatingId`
    LIMIT 1);

DELETE FROM `Rating`
WHERE NOT EXISTS (SELECT 1
    FROM `LoanApplicationRating` `lar`
    WHERE `lar`.`RatingId` = `Rating`.`RatingId`
    LIMIT 1)
AND NOT EXISTS (SELECT 1
    FROM `RatingLog` `rl`
    WHERE `rl`.`RatingId` = `Rating`.`RatingId`
    LIMIT 1);
END$$
DELIMITER ;
