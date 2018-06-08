CREATE PROCEDURE [jcu].[sForm]
	@formId UNIQUEIDENTIFIER
AS
SELECT [FormId], [UserId], [FormTypeId], [Style], [Content], [CreateTimestamp], [UpdateTimestamp]
FROM [jcu].[Form]
WHERE [FormId] = @formId
;