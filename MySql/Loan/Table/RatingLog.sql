CREATE TABLE `RatingLog` (
  `RatingLogId` binary(16) NOT NULL,
  `RatingId` binary(16) NOT NULL,
  `Value` double DEFAULT NULL,
  `Description` varchar(8000) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`RatingLogId`),
  KEY `RatingId` (`RatingId`),
  CONSTRAINT `FK_RatingLog_To_Rating` FOREIGN KEY (`RatingId`) REFERENCES `Rating` (`RatingId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
