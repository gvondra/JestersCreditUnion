DECLARE @domainId UNIQUEIDENTIFIER = 'c30cb9cd-c0a7-4044-b76a-d3c6a97caf08';
DECLARE @typeId UNIQUEIDENTIFIER = 'a10ce4bd-47a2-447f-8b26-3824b3a68459';
DECLARE @statusId UNIQUEIDENTIFIER = 'fbb6b62b-5507-4906-b770-4a9deb931cb2';

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