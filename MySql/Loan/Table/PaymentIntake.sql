CREATE TABLE `PaymentIntake` (
  `PaymentIntakeId` binary(16) NOT NULL,
  `LoanId` binary(16) NOT NULL,
  `PaymentId` binary(16) NULL,
  `TransactionNumber` varchar(128) NOT NULL,
  `Date` date NOT NULL,
  `Amount` decimal(8,2) NOT NULL,
  `Status` smallint NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (now()),
  `CreateUserId` varchar(64) NOT NULL,
  `UpdateUserId` varchar(64) NOT NULL,
  PRIMARY KEY (`PaymentIntakeId`),
  KEY `LoanId` (`LoanId`),
  KEY `PaymentId` (`PaymentId`),
  KEY `Status` (`Status`,`UpdateTimestamp` DESC),
  CONSTRAINT `FK_PaymentIntake_To_Loan` FOREIGN KEY (`LoanId`) REFERENCES `Loan` (`LoanId`),
  CONSTRAINT `FK_PaymentIntake_To_Payment` FOREIGN KEY (`PaymentId`) REFERENCES `Payment` (`PaymentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
