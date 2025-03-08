CREATE TABLE `IdentificationCard` (
  `IdentificationCardId` binary(16) NOT NULL,
  `InitializationVector` binary(16) NOT NULL,
  `Key` varbinary(256) NOT NULL,
  `MasterKeyName` varchar(64) NOT NULL DEFAULT (_utf8mb4''),
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`IdentificationCardId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
