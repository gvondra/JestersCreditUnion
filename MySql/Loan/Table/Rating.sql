CREATE TABLE `Rating` (
  `RatingId` binary(16) NOT NULL,
  `Value` double NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`RatingId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
