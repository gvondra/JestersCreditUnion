CREATE PROCEDURE [ln].[GetIdentificationCard_by_LoanApplicationUserId]
	@userId UNIQUEIDENTIFIER
AS
SELECT
[ic].[IdentificationCardId], [ic].[InitializationVector], [ic].[Key],
[ic].[CreateTimestamp], [ic].[UpdateTimestamp]
FROM [ln].[IdentificationCard] [ic]
INNER JOIN [ln].[LoanApplication] [app] on [ic].[IdentificationCardId] = [app].[BorrowerIdentificationCardId]
WHERE [app].[UserId] = @userId
;