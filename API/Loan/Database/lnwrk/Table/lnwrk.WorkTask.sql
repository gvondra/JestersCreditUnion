CREATE TABLE [lnwrk].[WorkTask]
(
	[WorkTaskId] UNIQUEIDENTIFIER NOT NULL,
	[AssignedDate] DATE NULL,
	[AssignedToUserId] UNIQUEIDENTIFIER NULL,
	[ClosedDate] DATE NULL,
	[Title] NVARCHAR(512) NOT NULL,
	[TypeCode] NVARCHAR(128) NOT NULL,
	[TypeTitle] NVARCHAR(512) NOT NULL,
	[StatusCode] NVARCHAR(128) NOT NULL,
	[StatusName] NVARCHAR(128) NOT NULL,
	[CreateTimestamp] DATETIME2(4) NOT NULL
)
