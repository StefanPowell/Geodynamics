DROP PROCEDURE [dbo].[usp_Get_Earthquake];
Go

DROP TRIGGER Trigger_SaveEarthquakeToFindWaveform_OnInsert;
Go

DROP TABLE [dbo].[PendingWaveformDataRetrieval];
Go

DROP PROCEDURE [dbo].[InsertCrustStress];
Go

DROP TYPE [dbo].[CrustStress];
Go

DROP TABLE [dbo].[world_stress_map];
Go

DROP TABLE [dbo].[waveTravelTime];
Go