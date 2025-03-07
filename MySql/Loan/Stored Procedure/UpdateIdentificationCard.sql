DROP PROCEDURE IF EXISTS `UpdateIdentificationCard`;
DELIMITER $$
CREATE PROCEDURE `UpdateIdentificationCard`(
`id` CHAR(16),
`initializationVector` VARBINARY(16),
`key` VARBINARY(256),
`masterKeyName` VARCHAR(64),
OUT `timestamp` TIMESTAMP
)
BEGIN	
	SET @timestamp = UTC_TIMESTAMP(4);
	UPDATE `IdentificationCard`
	SET `InitializationVector` = @initializationVector,
	`Key` = @key,
	`MasterKeyName` = @masterKeyName,
	`UpdateTimestamp` = @timestamp
	WHERE `IdentificationCardId` = @id;
END$$
DELIMITER ;
