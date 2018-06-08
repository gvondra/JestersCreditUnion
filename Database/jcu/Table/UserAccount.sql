CREATE TABLE [jcu].[UserAccount]
(
	[UserAccountId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [SubscriberId] NVARCHAR(1000) NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [FK_UserAccount_To_User] FOREIGN KEY ([UserId]) REFERENCES [jcu].[User]([UserId])
)

GO

CREATE UNIQUE INDEX [IX_UserAccount_SubscriberId] ON [jcu].[UserAccount] ([SubscriberId])

GO

CREATE INDEX [IX_UserAccount_UserId] ON [jcu].[UserAccount] ([UserId])
