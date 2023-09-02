CREATE PROCEDURE [ln].[CreateLoanApplicationComment]
	@id UNIQUEIDENTIFIER OUT,
	@loanApplicationId UNIQUEIDENTIFIER,
	@userId UNIQUEIDENTIFIER,
	@isInternal BIT,
	@text NVARCHAR(MAX),
	@timestamp DATETIME2(4) OUT
AS
BEGIN
	SET @id = NEWID();
	SET @timestamp = SYSUTCDATETIME();
	INSERT INTO [ln].[LoanApplicationComment] ([LoanApplicationCommentId], [LoanApplicationId], [UserId], [IsInternal], [Text], [CreateTimestamp])
	VALUES (@id, @loanApplicationId, @userId, @isInternal, @text, @timestamp);
END