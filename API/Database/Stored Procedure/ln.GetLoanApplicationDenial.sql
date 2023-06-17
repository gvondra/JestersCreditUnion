CREATE PROCEDURE [ln].[GetLoanApplicationDenial]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1 
	[LoanApplicationDenialId], [UserId], [Reason], [Date], [Text], [CreateTimestamp]
FROM [ln].[LoanApplicationDenial]
WHERE [LoanApplicationDenialId] = @id
;