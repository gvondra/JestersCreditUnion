CREATE TABLE [jcu].[UserGroup]
(
	[UserId] UNIQUEIDENTIFIER NOT NULL , 
    [GroupId] UNIQUEIDENTIFIER NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(),
    PRIMARY KEY ([GroupId], [UserId]), 
    CONSTRAINT [FK_UserGroup_To_User] FOREIGN KEY ([UserId]) REFERENCES [jcu].[User]([UserId]), 
    CONSTRAINT [FK_UserGroup_To_Group] FOREIGN KEY ([GroupId]) REFERENCES [jcu].[Group]([GroupId])
)

GO

CREATE INDEX [IX_UserGroup_UserId] ON [jcu].[UserGroup] ([UserId])

GO

CREATE INDEX [IX_UserGroup_GroupId] ON [jcu].[UserGroup] ([GroupId])
