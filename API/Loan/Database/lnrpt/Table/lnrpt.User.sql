﻿CREATE TABLE [lnrpt].[User]
(
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(1024) NOT NULL,
	[EmailAddress] NVARCHAR(2048) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY NONCLUSTERED ([UserId])
)