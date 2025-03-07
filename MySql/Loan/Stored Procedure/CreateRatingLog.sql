DROP PROCEDURE IF EXISTS `CreateRatingLog`;
DELIMITER $$
CREATE PROCEDURE `CreateRatingLog`(
OUT `id` CHAR(16),
`ratingId` CHAR(16),
`value` DOUBLE,
`description` VARCHAR(8000),
OUT `timestamp` TIMESTAMP
)
BEGIN
SET @id = UUID();
SET @timestamp = UTC_TIMESTAMP(4);
INSERT INTO `RatingLog` (`RatingLogId`, `RatingId`, `Value`, `Description`, `CreateTimestamp`)
VALUES (@id, @ratingId, @value, @description, @timestamp);
END$$
DELIMITER ;
