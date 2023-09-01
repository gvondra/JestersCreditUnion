CREATE PROCEDURE [ln].[GetIdentificationCard]
	@id UNIQUEIDENTIFIER
AS
SELECT TOP 1
[IdentificationCardId],[InitializationVector],[Key],[MasterKeyName],
[CreateTimestamp],[UpdateTimestamp]
FROM [ln].[IdentificationCard]
WHERE [IdentificationCardId] = @id
;