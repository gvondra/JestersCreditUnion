CREATE PROCEDURE [jcu].[uOrganization]
	@id UNIQUEIDENTIFIER, 
    @name NVARCHAR(500),
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[Organization]
	SET [Name] = @name,
		[UpdateTimestamp] = @timestamp
	WHERE [OrganizationId] = @id;
END