CREATE PROCEDURE [jcu].[sOrganizationSearch]
	@wildCardValue NVARCHAR(500)
AS
SELECT [OrganizationId], [Name], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Organization]
WHERE [Name] LIKE @wildCardValue ESCAPE '\';