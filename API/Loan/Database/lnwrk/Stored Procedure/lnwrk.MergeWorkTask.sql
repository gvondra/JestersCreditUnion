CREATE PROCEDURE [lnwrk].[MergeWorkTask]
AS
BEGIN
MERGE INTO [lnrpt].[WorkTask] as [tgt]
USING (SELECT [Title]
	FROM [lnwrk].[WorkTask]
	GROUP BY [Title])
	AS [src] ([Title])
ON [tgt].[Title] = [src].[Title]
WHEN NOT MATCHED THEN
	INSERT ([Title])
	VALUES ([src].[Title])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
END