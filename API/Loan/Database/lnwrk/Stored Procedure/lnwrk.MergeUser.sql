CREATE PROCEDURE [lnwrk].[MergeUser]
AS
BEGIN
MERGE INTO [lnrpt].[User] as [tgt]
USING (SELECT [UserId], [Name], [EmailAddress]
	FROM [lnwrk].[User]) as [src] ([UserId], [Name], [EmailAddress])
ON [tgt].[UserId] = [src].[UserId]
WHEN NOT MATCHED THEN
	INSERT ([UserId], [Name], [EmailAddress])
	VALUES ([src].[UserId], [src].[Name], [src].[EmailAddress])
WHEN MATCHED THEN
	UPDATE SET
	[Name] = [src].[Name],
	[EmailAddress] = [src].[EmailAddress]
;
END