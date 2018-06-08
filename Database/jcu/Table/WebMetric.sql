CREATE TABLE [jcu].[WebMetric]
(
	[WebMetricId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Url] NVARCHAR(500) NOT NULL, 
    [Method] NVARCHAR(50) NOT NULL,
    [CreateTimestamp] DATETIME NOT NULL, 
    [Duration] FLOAT NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [Controller] NVARCHAR(50) NOT NULL
)

GO

CREATE INDEX [IX_WebMetric_CreateTimestamp_WebMetricId] ON [jcu].[WebMetric] ([CreateTimestamp], [WebMetricId])

GO

CREATE INDEX [IX_WebMetric_CreateTimestamp] ON [jcu].[WebMetric] ([CreateTimestamp])
