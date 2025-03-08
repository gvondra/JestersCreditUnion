CREATE TABLE `LoanApplicationRating` (
  `LoanApplicationId` binary(16) NOT NULL,
  `RatingId` binary(16) NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`LoanApplicationId`),
  KEY `RatingId` (`RatingId`),
  CONSTRAINT `FK_LoanApplicationRating_To_LoanApplication` FOREIGN KEY (`LoanApplicationId`) REFERENCES `LoanApplication` (`LoanApplicationId`),
  CONSTRAINT `FK_LoanApplicationRating_To_Rating` FOREIGN KEY (`RatingId`) REFERENCES `Rating` (`RatingId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
