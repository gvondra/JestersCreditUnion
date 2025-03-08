CREATE TABLE `Transaction` (
  `TransactionId` binary(16) NOT NULL,
  `LoanId` binary(16) NOT NULL,
  `Date` date NOT NULL,
  `Type` smallint NOT NULL,
  `Amount` decimal(11,2) NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`TransactionId`),
  KEY `LoanId` (`LoanId`),
  CONSTRAINT `FK_Transaction_To_Loan` FOREIGN KEY (`LoanId`) REFERENCES `Loan` (`LoanId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
