CREATE TABLE [jcu].[Address]
(
	[AddressId] UNIQUEIDENTIFIER NOT NULL,
	[Hash] BINARY(64) NOT NULL,
	[Recipient] VARCHAR(512) NOT NULL,
	[Attention] VARCHAR(512) NOT NULL,
	[Delivery] VARCHAR(512) NOT NULL,
	[Secondary] VARCHAR(512) NOT NULL,
	[City] VARCHAR(512) NOT NULL,
	[State] CHAR(2) NOT NULL,
	[PostalCode] VARCHAR(9) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_Address_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_Address] PRIMARY KEY NONCLUSTERED ([AddressId])
)

GO

CREATE INDEX [IX_Address_Hash] ON [jcu].[Address] ([Hash]);
