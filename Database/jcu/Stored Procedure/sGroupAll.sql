CREATE PROCEDURE [jcu].[sGroupAll]
AS
SELECT [GroupId], [Name], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Group]
;

