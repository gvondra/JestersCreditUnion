DROP PROCEDURE IF EXISTS `CreateLoanHistory`;
DELIMITER $$
CREATE PROCEDURE `CreateLoanHistory`(
OUT `id` CHAR(16),
`loanId` CHAR(16),
`initialDisbursementDate` DATE,
`firstPaymentDue` DATE,
`nextPaymentDue` DATE,
`status` SMALLINT,
`balance` DECIMAL(11,2),
OUT `timestamp` TIMESTAMP
)
BEGIN
SET @id = UUID();
SET @timestamp = UTC_TIMESTAMP(4);
INSERT INTO `LoanHistory` (`LoanHistoryId`, `LoanId`, `InitialDisbursementDate`, `FirstPaymentDue`, `NextPaymentDue`, `Status`, `Balance`,
`CreateTimestamp`, `UpdateTimestamp`)
VALUES (@id, @loanId, @initialDisbursementDate, @firstPaymentDue, @nextPaymentDue, @status, @balance,
@timestamp, @timestamp);
END$$
DELIMITER ;
