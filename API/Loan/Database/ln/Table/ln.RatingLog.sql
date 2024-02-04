CREATE TABLE [ln].[RatingLog]
(
	[RatingLogId] UNIQUEIDENTIFIER NOT NULL,
	[RatingId] UNIQUEIDENTIFIER NOT NULL,
	[Value] FLOAT NULL,
	[Description] VARCHAR(MAX) NOT NULL,
	[CreateTimestamp] DATETIME2(4) CONSTRAINT [DF_RatingLog_CreateTimestamp] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	CONSTRAINT [PK_RatingLog] PRIMARY KEY NONCLUSTERED ([RatingLogId]), 
    CONSTRAINT [FK_RatingLog_To_Rating] FOREIGN KEY ([RatingId]) REFERENCES [ln].[Rating]([RatingId])
)
WITH (DATA_COMPRESSION = PAGE)

GO

CREATE CLUSTERED INDEX [IX_RatingLog_RatingId] ON [ln].[RatingLog] ([RatingId])
