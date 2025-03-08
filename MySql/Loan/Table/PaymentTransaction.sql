CREATE TABLE `PaymentTransaction` (
  `PaymentId` binary(16) NOT NULL,
  `TransactionId` binary(16) NOT NULL,
  `TermNumber` smallint NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (now()),
  PRIMARY KEY (`PaymentId`,`TransactionId`),
  CONSTRAINT `FK_PaymentTransaction_To_Payment` FOREIGN KEY (`PaymentId`) REFERENCES `Payment` (`PaymentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
