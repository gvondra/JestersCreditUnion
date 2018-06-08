CREATE PROCEDURE [jcu].[uGroup]
	@id UNIQUEIDENTIFIER, 
    @name NVARCHAR(100),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[Group]
	SET [Name] = @name,
		[UpdateTimestamp] = @timestamp
	WHERE [GroupId] = @id;
END