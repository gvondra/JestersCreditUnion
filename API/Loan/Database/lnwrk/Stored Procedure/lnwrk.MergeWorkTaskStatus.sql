CREATE PROCEDURE [lnwrk].[MergeWorkTaskStatus]
AS
BEGIN
MERGE INTO [lnrpt].[WorkTaskStatus] as [tgt]
USING (SELECT [StatusCode], [StatusName]
	FROM [lnwrk].[WorkTask]
	GROUP BY [StatusCode], [StatusName])
	AS [src] ([Code], [Name])
ON [tgt].[Code] = [src].[Code]
WHEN NOT MATCHED THEN
	INSERT ([Code], [Name])
	VALUES ([src].[Code], [src].[Name])
WHEN MATCHED THEN
	UPDATE SET [Name] = [src].[Name]
;
END