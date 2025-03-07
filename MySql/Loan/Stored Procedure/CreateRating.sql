DROP PROCEDURE IF EXISTS `CreateRating`;
DELIMITER $$
CREATE PROCEDURE `CreateRating`(
OUT `id` CHAR(16),
`value` DOUBLE,
OUT `timestamp` TIMESTAMP
)
BEGIN
SET @id = UUID();
SET @timestamp = UTC_TIMESTAMP(4);
INSERT INTO `Rating` (`RatingId`, `Value`, `CreateTimestamp`)
VALUES (@id, @value, @timestamp);
END$$
DELIMITER ;
