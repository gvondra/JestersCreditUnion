CREATE PROCEDURE [lnwrk].[MergeLoanAgreement]
AS
BEGIN
MERGE INTO [lnrpt].[LoanAgreement] as [tgt]
USING (SELECT [CreateDate], [AgreementDate], [InterestRate], [PaymentAmount]
	FROM [lnwrk].[LoanAgreement]) as [src] ([CreateDate], [AgreementDate], [InterestRate], [PaymentAmount])
ON [tgt].[CreateDate] = [src].[CreateDate]
	and [tgt].[AgreementDate] = [src].[AgreementDate]
	and [tgt].[InterestRate] = [src].[InterestRate]
	and [tgt].[PaymentAmount] = [src].[PaymentAmount]
WHEN NOT MATCHED THEN
	INSERT ([CreateDate], [AgreementDate], [InterestRate], [PaymentAmount])
	VALUES ([src].[CreateDate], [src].[AgreementDate], [src].[InterestRate], [src].[PaymentAmount])
;
END