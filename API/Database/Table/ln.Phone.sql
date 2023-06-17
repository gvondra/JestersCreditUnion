﻿CREATE TABLE [ln].[Phone]
(
	[PhoneId] UNIQUEIDENTIFIER NOT NULL,
	[Number] VARCHAR(15) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Phone_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL, 
    CONSTRAINT [PK_Phone] PRIMARY KEY NONCLUSTERED ([PhoneId])
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Phone_Number] ON [ln].[Phone] ([Number])
