CREATE PROCEDURE [jcu].[sGroup]
	@id UNIQUEIDENTIFIER
AS
SELECT [GroupId], [Name], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Group]
WHERE [GroupId] = @id
;