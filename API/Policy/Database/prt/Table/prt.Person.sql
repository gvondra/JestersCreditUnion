﻿CREATE TABLE [prt].[Person]
(
	[PartyId] UNIQUEIDENTIFIER NOT NULL,
	[Nickname] NVARCHAR(1024) CONSTRAINT [DF_Person_Nickname] DEFAULT 0 NOT NULL,
	[BirthDate] DATE NULL,
	CONSTRAINT [PK_Person] PRIMARY KEY NONCLUSTERED ([PartyId])
)