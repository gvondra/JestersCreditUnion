CREATE TABLE `LoanApplicationDenial` (
  `LoanApplicationId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `Reason` smallint NOT NULL,
  `Date` date NOT NULL,
  `Text` varchar(8000) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  PRIMARY KEY (`LoanApplicationId`),
  CONSTRAINT `FK_LoanApplicationDenial_To_LoanApplication` FOREIGN KEY (`LoanApplicationId`) REFERENCES `LoanApplication` (`LoanApplicationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
