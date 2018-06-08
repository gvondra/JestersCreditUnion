CREATE PROCEDURE [jcu].[uUser]
	@id UNIQUEIDENTIFIER, 
    @fullName NVARCHAR(100), 
    @shortName NVARCHAR(80), 
    @birthDate DATE, 
    @emailAddress NVARCHAR(500), 
    @phoneNumber CHAR(13), 
	@timestamp DATETIME OUT
AS
BEGIN
	SET @timestamp = GetDate();
	UPDATE [jcu].[User]
	SET [FullName] = @fullName, 
		[ShortName] = @shortName, 
		[BirthDate] = @birthDate, 
		[EmailAddress] = @emailAddress, 
		[PhoneNumber] = @phoneNumber, 
		[UpdateTimestamp] = @timestamp
	WHERE [UserId] = @id;
END