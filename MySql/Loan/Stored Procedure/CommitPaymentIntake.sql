DROP PROCEDURE IF EXISTS `CommitPaymentIntake`
DELIMITER %%
CREATE PROCEDURE `CommitPaymentIntake`(
`intakeStatusFilter` SMALLINT,
`maxTimestamp` TIMESTAMP,
`intakeStatus` SMALLINT,
`paymentStatus` SMALLINT,
`userId` VARCHAR(64)
)
BEGIN
DECLARE `done` INT DEFAULT FALSE;
DECLARE `timestamp` TIMESTAMP DEFAULT UTC_TIMESAMP(4);
DECLARE `paymentId` CHAR(16);
DECLARE `paymentIntakeId` CHAR(16);
DECLARE `loanId` CHAR(16);
DECLARE `transactionNumber` VARCHAR(128);
DECLARE `date` DATE;
DECLARE `amount` DECIMAL(8,2);

DECLARE intakeCursor CURSOR
FOR SELECT `PaymentIntakeId`, `LoanId`, `TransactionNumber`, `Date`, `Amount`
FROM `PaymentIntake`
WHERE `Status` = @intakeStatusFilter
AND `UpdateTimestamp` < @maxTimestamp;
DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

OPEN intakeCursor;
read_loop: LOOP
	FETCH NEXT FROM intakeCursor INTO `paymentIntakeId`, `loanId`, `transactionNumber`, `date`, `amount`;
    IF done THEN
      LEAVE read_loop;
    END IF;
	SET @paymentId = UUID();
	INSERT INTO `Payment` (`PaymentId`, `LoanId`, `TransactionNumber`, `Date`, `Amount`, `Status`, `CreateTimestamp`, `UpdateTimestamp`)
	VALUES(@paymentId, @loanId, @transactionNumber, @date, @amount, @paymentStatus, @timestamp, @timestamp);

	UPDATE `PaymentIntake`
	SET `Status` = @intakeStatus,
	`PaymentId` = @paymentId,
	`UpdateTimestamp` = @timestamp,
	`UpdateUserId` = @userId
	WHERE `PaymentIntakeId` = @paymentIntakeId;

END LOOP;
CLOSE intakeCursor;
	
END%%
DELIMITER ;