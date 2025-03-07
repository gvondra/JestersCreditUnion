DROP PROCEDURE IF EXISTS `CreateIdentificationCard`;
DELIMITER %%
CREATE PROCEDURE `CreateIdentificationCard`(
`id` CHAR(16),
`initializationVector` VARBINARY(16),
`key` VARBINARY(256),
`masterKeyName` VARCHAR(64),
OUT `timestamp` TIMESTAMP
)
BEGIN	
	SET @timestamp = UTC_TIMESTAMP(4);
	INSERT INTO `IdentificationCard` (`IdentificationCardId`,`InitializationVector`,`Key`,`MasterKeyName`,
	`CreateTimestamp`,`UpdateTimestamp`)
	VALUES (@id, @initializationVector, @key, @masterKeyName,
	@timestamp, @timestamp);
END%%
DELIMITER ;