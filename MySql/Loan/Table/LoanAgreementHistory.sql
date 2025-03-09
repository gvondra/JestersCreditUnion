CREATE TABLE `LoanAgreementHistory` (
  `LoanAgreementHistoryId` binary(16) NOT NULL,
  `LoanId` binary(16) NOT NULL,
  `Status` smallint NOT NULL,
  `CreateDate` date NOT NULL,
  `AgreementDate` date DEFAULT NULL,
  `BorrowerName` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `BorrowerBirthDate` date NOT NULL,
  `BorrowerAddressId` binary(16) DEFAULT NULL,
  `BorrowerEmailAddressId` binary(16) DEFAULT NULL,
  `BorrowerPhoneId` binary(16) DEFAULT NULL,
  `CoBorrowerName` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CoBorrowerBirthDate` date DEFAULT NULL,
  `CoBorrowerAddressId` binary(16) DEFAULT NULL,
  `CoBorrowerEmailAddressId` binary(16) DEFAULT NULL,
  `CoBorrowerPhoneId` binary(16) DEFAULT NULL,
  `OriginalAmount` decimal(11,2) NOT NULL,
  `OriginalTerm` smallint NOT NULL,
  `InterestRate` decimal(5,4) NOT NULL,
  `PaymentAmount` decimal(7,2) NOT NULL,
  `PaymentFrequency` smallint NOT NULL,
  `CreateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  `UpdateTimestamp` timestamp NOT NULL DEFAULT (utc_timestamp()),
  PRIMARY KEY (`LoanAgreementHistoryId`),
  KEY `LoanId` (`LoanId`),
  KEY `CreateTimestamp` (`CreateTimestamp` DESC,`LoanId`),
  CONSTRAINT `FK_LoanAgreementHistory_LoanAgreement` FOREIGN KEY (`LoanId`) REFERENCES `LoanAgreement` (`LoanId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
