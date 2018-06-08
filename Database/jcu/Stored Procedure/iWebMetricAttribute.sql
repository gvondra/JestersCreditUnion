CREATE PROCEDURE [jcu].[iWebMetricAttribute]
	@id INT OUT, 
    @webMetricId INT, 
    @key NVARCHAR(500), 
    @value NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO [jcu].[WebMetricAttribute] ([WebMetricId], [Key], [Value])
	VALUES (@webMetricId, @key, @value);
	SET @id = SCOPE_IDENTITY();
END
