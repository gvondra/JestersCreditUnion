CREATE PROCEDURE [jcu].[sOrganization]
	@id UNIQUEIDENTIFIER
AS
SELECT [OrganizationId], [Name], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Organization]
WHERE [OrganizationId] = @id;