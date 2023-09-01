CREATE PROCEDURE [ln].[GetIdentificationCard]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1
[IdentificationCardId],[InitializationVector],[Key],
[CreateTimestamp],[UpdateTimestamp]
FROM [ln].[IdentificationCard]
WHERE [IdentificationCardId] = @id
;