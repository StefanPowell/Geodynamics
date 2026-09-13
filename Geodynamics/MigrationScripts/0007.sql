CREATE PROCEDURE [dbo].[usp_Feature_Get_QuakesNeedingWaveform]
AS
BEGIN

	SELECT 
		[FeatureId]
	FROM [EarthQuake].[dbo].[PendingWaveformDataRetrieval] 

END;
Go