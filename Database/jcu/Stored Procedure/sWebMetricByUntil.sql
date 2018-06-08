CREATE PROCEDURE [jcu].[sWebMetricByUntil]
	@until DATETIME,
	@rowLimit INT,
	@offset INT
AS
BEGIN 
	SELECT [WebMetricId], [Url], [Method], [CreateTimestamp], [Duration], [Status], [Controller]
	FROM [jcu].[WebMetric]
	WHERE [WebMetric].[CreateTimestamp] < @until
	ORDER BY [WebMetric].[CreateTimestamp] DESC, [WebMetric].[WebMetricId] DESC
		OFFSET @offset ROWS
		FETCH NEXT @rowLimit ROWS ONLY
	;	

	SELECT [WMA].[WebMetricAttributeId], [WMA].[WebMetricId], [WMA].[Key], [WMA].[Value] 
	FROM [jcu].[WebMetricAttribute] [WMA]
	WHERE [WMA].[WebMetricId] IN (
		SELECT [WebMetricId]
		FROM [jcu].[WebMetric]
		WHERE [WebMetric].[CreateTimestamp] < @until
		ORDER BY [WebMetric].[CreateTimestamp] DESC, [WebMetric].[WebMetricId] DESC
			OFFSET @offset ROWS
			FETCH NEXT @rowLimit ROWS ONLY	
	)
	;	
END