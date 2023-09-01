CREATE TABLE [ln].[IdentificationCard]
(
	[IdentificationCardId] UNIQUEIDENTIFIER NOT NULL,
	[InitializationVector] BINARY(16) NULL,
	[Key] BINARY(256) NULL,
	[MasterKeyName] VARCHAR(64) CONSTRAINT [DF_IdentificationCard_MasterKeyName] DEFAULT '' NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_IdentificationCard_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[UpdateTimestamp] DATETIME2(4) CONSTRAINT [DF_IdentificationCard_UpdateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_IdentificationCard] PRIMARY KEY NONCLUSTERED ([IdentificationCardId])
)
