CREATE TABLE `Payment` (
  `PaymentId` binary(16) NOT NULL,
  `LoanId` binary(16) NOT NULL,
  `TransactionNumber` varchar(128) NOT NULL,
  `Date` date NOT NULL,
  `Amount` decimal(8,2) NOT NULL,
  `Status` smallint NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`PaymentId`),
  UNIQUE KEY `IX_Payment_Date_LoanId_TransactionNumber` (`Date` DESC,`LoanId`,`TransactionNumber`),
  KEY `Status` (`Status`),
  KEY `LoanId` (`LoanId`),
  CONSTRAINT `FK_Payment_To_Loan` FOREIGN KEY (`LoanId`) REFERENCES `Loan` (`LoanId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
