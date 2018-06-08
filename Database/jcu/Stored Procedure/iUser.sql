CREATE PROCEDURE [jcu].[iUser]
	@id UNIQUEIDENTIFIER OUT, 
    @fullName NVARCHAR(100), 
    @shortName NVARCHAR(80), 
    @birthDate DATE, 
    @emailAddress NVARCHAR(500), 
    @phoneNumber CHAR(13), 
	@timestamp DATETIME OUT
AS
BEGIN
	SET @id = NewId();
	SET @timestamp = GetDate();
	INSERT INTO [jcu].[User] ([UserId], [FullName], [ShortName], [BirthDate], [EmailAddress], [PhoneNumber], [CreateTimestamp], [UpdateTimestamp])
	VALUES (@id, @fullName, @shortName, @birthDate, @emailAddress, @phoneNumber, @timestamp, @timestamp);
END
