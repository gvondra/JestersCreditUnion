DROP PROCEDURE IF EXISTS `CreateRatingLog`;
DELIMITER $$
CREATE PROCEDURE `CreateRatingLog`(
OUT `id` BINARY(16),
`ratingId` BINARY(16),
`value` DOUBLE,
`description` VARCHAR(8000),
OUT `timestamp` TIMESTAMP
)
BEGIN
SET `id` = UUID_TO_BIN(UUID());
SET `timestamp` = UTC_TIMESTAMP(4);
INSERT INTO `RatingLog` (`RatingLogId`, `RatingId`, `Value`, `Description`, `CreateTimestamp`)
VALUES (`id`, `ratingId`, `value`, `description`, `timestamp`);
END$$
DELIMITER ;
