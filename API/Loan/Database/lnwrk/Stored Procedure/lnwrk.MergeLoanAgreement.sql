CREATE PROCEDURE [lnwrk].[MergeLoanAgreement]
AS
BEGIN
MERGE INTO [lnrpt].[LoanAgreement] as [tgt]
USING (SELECT [Hash], [CreateDate], [AgreementDate], [InterestRate], [PaymentAmount]
	FROM [lnwrk].[LoanAgreement]) as [src] ([Hash], [CreateDate], [AgreementDate], [InterestRate], [PaymentAmount])
ON [tgt].[Hash] = [src].[Hash]
	AND [tgt].[CreateDate] = [src].[CreateDate]
	and (([tgt].[AgreementDate] is NULL and [src].[AgreementDate] is NULL) or [tgt].[AgreementDate] = [src].[AgreementDate])
	and [tgt].[InterestRate] = [src].[InterestRate]
	and [tgt].[PaymentAmount] = [src].[PaymentAmount]
WHEN NOT MATCHED THEN
	INSERT ([Hash], [CreateDate], [AgreementDate], [InterestRate], [PaymentAmount])
	VALUES ([src].[Hash], [src].[CreateDate], [src].[AgreementDate], [src].[InterestRate], [src].[PaymentAmount])
;
END