DECLARE @domainId UNIQUEIDENTIFIER = 'c30cb9cd-c0a7-4044-b76a-d3c6a97caf08';
DECLARE @typeId UNIQUEIDENTIFIER = '34517cc6-42b4-4dcc-9279-369ac3ac1786';
DECLARE @statusId UNIQUEIDENTIFIER = 'ed9fea32-855d-4f95-92e8-c6b85ec08ab5';

BEGIN TRANSACTION
BEGIN TRY

DELETE FROM [blwt].[WorkTaskComment]
WHERE EXISTS (SELECT TOP 1 1
  FROM [blwt].[WorkTask] [tsk]
  WHERE [tsk].[DomainId] = @domainId
    AND [tsk].[WorkTaskTypeId] = @typeId
    AND [tsk].[WorkTaskStatusId] = @statusId
    AND [tsk].[AssignedToUserId] = ''
    AND [tsk].[WorkTaskId] = [blwt].[WorkTaskComment].[WorkTaskId])
;

DELETE FROM [blwt].[WorkTaskContext]
WHERE [DomainId] = @domainId
AND EXISTS (SELECT TOP 1 1
  FROM [blwt].[WorkTask] [tsk]
  WHERE [tsk].[DomainId] = @domainId
    AND [tsk].[WorkTaskTypeId] = @typeId
    AND [tsk].[WorkTaskStatusId] = @statusId
    AND [tsk].[AssignedToUserId] = ''
    AND [tsk].[WorkTaskId] = [blwt].[WorkTaskContext].[WorkTaskId])
;

DELETE FROM [blwt].[WorkTask]
WHERE [DomainId] = @domainId
AND [WorkTaskTypeId] = @typeId
AND [WorkTaskStatusId] = @statusId
AND [AssignedToUserId] = ''
;

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION;
THROW
END CATCH