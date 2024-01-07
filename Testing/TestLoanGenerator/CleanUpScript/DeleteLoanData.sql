DECLARE @loanApplicationId UNIQUEIDENTIFIER;
DECLARE @loanId UNIQUEIDENTIFIER;

DECLARE loanApplicationCursor CURSOR
FOR SELECT [app].[LoanApplicationId], [l].[LoanId]
FROM [ln].[LoanApplication] [app]
LEFT JOIN [ln].[Loan] [l] on [app].[LoanApplicationId] = [l].[LoanApplicationId]
WHERE [app].[Purpose] = 'Automated test'
;

OPEN loanApplicationCursor;

FETCH NEXT FROM loanApplicationCursor
INTO @loanApplicationId, @loanId;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY

    IF @loanId IS NOT NULL
    BEGIN
        PRINT 'Loan id ' + CONVERT(VARCHAR(MAX), @loanId);

        DELETE FROM [ln].[PaymentTransaction]
        WHERE EXISTS (SELECT TOP 1 1 
        FROM [ln].[Payment] [pmt]
        WHERE [pmt].[PaymentId] = [ln].[PaymentTransaction].[PaymentId]
        AND [pmt].[LoanId] = @loanId);

        DELETE FROM [ln].[Transaction]
        WHERE [LoanId] = @loanId;

        DELETE FROM [ln].[Payment]
        WHERE [LoanId] = @loanId;

        DELETE FROM [ln].[LoanHistory]
        WHERE [LoanId] = @loanId;

        DELETE FROM [ln].[Loan]
        WHERE [LoanId] = @loanId;
        
        DELETE FROM [ln].[LoanAgreementHistory]
        WHERE [LoanId] = @loanId;

        DELETE FROM [ln].[LoanAgreement]
        WHERE [LoanId] = @loanId;
    END

    PRINT 'Loan application id ' + CONVERT(VARCHAR(MAX), @loanApplicationId);
    DELETE FROM [ln].[LoanApplicationDenial]
    WHERE [LoanApplicationId] = @loanApplicationId
    ;

    DELETE FROM [ln].[LoanApplicationComment]
    WHERE [LoanApplicationId] = @loanApplicationId
    ;

    DELETE FROM [ln].[LoanApplication]
    WHERE [LoanApplicationId] = @loanApplicationId
    ;

    COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
    END CATCH

    FETCH NEXT FROM loanApplicationCursor
    INTO @loanApplicationId, @loanId;
END

CLOSE loanApplicationCursor;
DEALLOCATE loanApplicationCursor;