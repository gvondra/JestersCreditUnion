CREATE PROCEDURE [lnwrk].[MergeWorkTaskCycle]
AS
BEGIN
MERGE INTO [lnrpt].[WorkTaskCycle] as [tgt]
USING (SELECT [CreateTimestamp], [AssignedDate], [AssignedToUserId], [ClosedDate], [TypeCode], [StatusCode],
	(SELECT [WorkTaskId] FROM [lnrpt].[WorkTask] WHERE [Title] = [lnwrk].[WorkTask].[Title]) [WorkTaskId]
	FROM [lnwrk].[WorkTask])
	as [src] ([CreateTimestamp], [AssignedDate], [AssignedToUserId], [ClosedDate], [TypeCode], [StatusCode], [WorkTaskId])
ON [tgt].[CreateDate] = CONVERT(DATE, [src].[CreateTimestamp])
AND (([tgt].[AssignedDate] is null and [src].[AssignedDate] is null) or [tgt].[AssignedDate] = [src].[AssignedDate])
AND (([tgt].[AssignedToUserId] is null and [src].[AssignedToUserId] is null) or [tgt].[AssignedToUserId] = [src].[AssignedToUserId])
AND (([tgt].[ClosedDate] is null and [src].[ClosedDate] is null) or [tgt].[ClosedDate] = [src].[ClosedDate])
AND [tgt].[TypeCode] = [src].[TypeCode]
AND [tgt].[StatusCode] = [src].[StatusCode]
AND [tgt].[WorkTaskId] = [src].[WorkTaskId]
WHEN NOT MATCHED THEN
	INSERT ([CreateDate], [AssignedDate], [AssignedToUserId], [ClosedDate], [WorkTaskId], [TypeCode], [StatusCode])
	VALUES (CONVERT(DATE, [src].[CreateTimestamp]), [src].[AssignedDate], [src].[AssignedToUserId], [src].[ClosedDate], [src].[WorkTaskId], [src].[TypeCode], [src].[StatusCode])
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
END