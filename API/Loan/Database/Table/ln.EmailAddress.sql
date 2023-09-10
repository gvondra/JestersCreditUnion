CREATE TABLE [ln].[EmailAddress]
(
	[EmailAddressId] UNIQUEIDENTIFIER NOT NULL,
	[Address] VARCHAR(1024) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_EmailAddress_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_EmailAddress] PRIMARY KEY NONCLUSTERED ([EmailAddressId])
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_EmailAddress_Address] ON [ln].[EmailAddress] ([Address])
