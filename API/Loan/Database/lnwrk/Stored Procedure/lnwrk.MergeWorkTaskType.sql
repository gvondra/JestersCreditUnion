CREATE PROCEDURE [lnwrk].[MergeWorkTaskType]
AS
BEGIN
MERGE INTO [lnrpt].[WorkTaskType] as [tgt]
USING (SELECT [TypeCode], [TypeTitle]
	FROM [lnwrk].[WorkTask]
	GROUP BY [TypeCode], [TypeTitle])
	AS [src] ([Code], [Title])
ON [tgt].[Code] = [src].[Code]
WHEN NOT MATCHED THEN
	INSERT ([Code], [Title])
	VALUES ([src].[Code], [src].[Title])
WHEN MATCHED THEN
	UPDATE SET [Title] = [src].[Title]
;
END