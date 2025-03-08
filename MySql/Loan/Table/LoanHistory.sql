CREATE TABLE `LoanHistory` (
  `LoanHistoryId` binary(16) NOT NULL,
  `LoanId` binary(16) NOT NULL,
  `InitialDisbursementDate` date DEFAULT NULL,
  `FirstPaymentDue` date DEFAULT NULL,
  `NextPaymentDue` date DEFAULT NULL,
  `Status` smallint NOT NULL DEFAULT (0),
  `Balance` decimal(11,2) DEFAULT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`LoanHistoryId`),
  KEY `LoanId` (`LoanId`),
  KEY `CreateTimestamp` (`CreateTimestamp` DESC,`LoanId`),
  CONSTRAINT `FK_LoanHistory_Loan` FOREIGN KEY (`LoanId`) REFERENCES `Loan` (`LoanId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
