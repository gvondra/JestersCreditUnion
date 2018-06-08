CREATE TABLE [jcu].[WebMetricAttribute]
(
	[WebMetricAttributeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WebMetricId] INT NOT NULL, 
    [Key] NVARCHAR(500) NOT NULL, 
    [Value] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_WebMetricAttribute_To_WebMetric] FOREIGN KEY ([WebMetricId]) REFERENCES [jcu].[WebMetric]([WebMetricId])
)

GO

CREATE INDEX [IX_WebMetricAttribute_WebMetricId] ON [jcu].[WebMetricAttribute] ([WebMetricId])
