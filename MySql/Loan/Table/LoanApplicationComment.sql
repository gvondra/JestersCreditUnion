CREATE TABLE `LoanApplicationComment` (
  `LoanApplicationCommentId` binary(16) NOT NULL,
  `LoanApplicationId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `IsInternal` tinyint(1) NOT NULL DEFAULT (1),
  `Text` varchar(8000) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  PRIMARY KEY (`LoanApplicationCommentId`),
  KEY `LoanApplicationId` (`LoanApplicationId`),
  CONSTRAINT `FK_LoanApplicationComment_To_LoanApplication` FOREIGN KEY (`LoanApplicationId`) REFERENCES `LoanApplication` (`LoanApplicationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
