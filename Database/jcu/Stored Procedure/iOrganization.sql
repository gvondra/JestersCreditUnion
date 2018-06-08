CREATE PROCEDURE [jcu].[iOrganization]
	@id UNIQUEIDENTIFIER OUT, 
    @name NVARCHAR(500),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[Organization] ([OrganizationId], [Name], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @name, @timestamp, @timestamp);
END