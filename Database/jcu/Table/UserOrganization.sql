CREATE TABLE [jcu].[UserOrganization]
(
	[UserId] UNIQUEIDENTIFIER NOT NULL , 
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(),
    PRIMARY KEY ([OrganizationId], [UserId]), 
    CONSTRAINT [FK_UserOrganization_To_User] FOREIGN KEY ([UserId]) REFERENCES [jcu].[User]([UserId]), 
    CONSTRAINT [FK_UserOrganization_To_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [jcu].[Organization]([OrganizationId])
)

GO

CREATE INDEX [IX_UserOrganization_UserId] ON [jcu].[UserOrganization] ([UserId])

GO

CREATE INDEX [IX_UserOrganization_OrganizationId] ON [jcu].[UserOrganization] ([OrganizationId])
