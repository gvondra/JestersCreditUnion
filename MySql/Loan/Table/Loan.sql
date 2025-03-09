CREATE TABLE `Loan` (
  `LoanId` binary(16) NOT NULL,
  `Number` varchar(16) NOT NULL,
  `LoanApplicationId` binary(16) NOT NULL,
  `InitialDisbursementDate` date DEFAULT NULL,
  `FirstPaymentDue` date DEFAULT NULL,
  `NextPaymentDue` date DEFAULT NULL,
  `Status` smallint NOT NULL DEFAULT (0),
  `Balance` decimal(11,2) DEFAULT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  PRIMARY KEY (`LoanId`),
  KEY `Number` (`Number`),
  KEY `LoanApplicationId` (`LoanApplicationId`),
  CONSTRAINT `FK_Loan_To_LoanAgreement` FOREIGN KEY (`LoanId`) REFERENCES `LoanAgreement` (`LoanId`),
  CONSTRAINT `FK_Loan_To_LoanApplication` FOREIGN KEY (`LoanApplicationId`) REFERENCES `LoanApplication` (`LoanApplicationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
