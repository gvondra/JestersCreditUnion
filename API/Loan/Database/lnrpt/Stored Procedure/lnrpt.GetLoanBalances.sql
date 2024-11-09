CREATE PROCEDURE [lnrpt].[GetLoanBalances]
AS
BEGIN
SELECT [Status], [Description]
FROM [lnrpt].[LoanStatus]
;

SELECT [l].[LoanId],[l].[Number],[l].[InitialDisbursementDate],[l].[FirstPaymentDue],[l].[NextPaymentDue]
FROM [lnrpt].[Loan] [l]
WHERE EXISTS (SELECT TOP 1 1 FROM [lnrpt].[LoanBalance] [lBal] WHERE [l].[LoanId] = [lBal].[LoanId])
;

SELECT [agmt].[LoanAgreementId],[agmt].[Hash],[agmt].[CreateDate],[agmt].[AgreementDate],[agmt].[InterestRate],[agmt].[PaymentAmount]
FROM [lnrpt].[LoanAgreement] [agmt]
WHERE EXISTS (SELECT TOP 1 1 FROM [lnrpt].[LoanBalance] [lBal] WHERE [agmt].[LoanAgreementId] = [lBal].[LoanAgreementId])
;

SELECT [lBal].[Id],[lBal].[Date],[lBal].[Balance],[lBal].[DaysPastDue],[lBal].[LoanId],[lBal].[LoanAgreementId],[lBal].[LoanStatus]
FROM [lnrpt].[LoanBalance] [lBal]
;
END