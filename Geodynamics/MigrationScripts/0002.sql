USE master;
GO

IF DB_ID('EarthQuake') IS NOT NULL
BEGIN
    ALTER DATABASE EarthQuake SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
END
GO

USE EarthQuake;
GO

DROP PROCEDURE IF EXISTS [dbo].[UpdateFaultStress];
DROP PROCEDURE IF EXISTS [dbo].[sp_Feature_Get_AllWithGridAndTime];
DROP PROCEDURE IF EXISTS [dbo].[InsertFeatures];
DROP PROCEDURE IF EXISTS [dbo].[GetLatestFeatures];

DROP TYPE IF EXISTS [dbo].[Properties];
DROP TYPE IF EXISTS [dbo].[Identifiers];
DROP TYPE IF EXISTS [dbo].[Geo];
DROP TYPE IF EXISTS [dbo].[FeatureType];
DROP TYPE IF EXISTS [dbo].[FaultStressType];
DROP TYPE IF EXISTS [dbo].[Coordinates];

DROP TABLE IF EXISTS [dbo].[Metadata];

DECLARE @constraintName NVARCHAR(200);
SELECT @constraintName = name
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('dbo.fault_coordinates');

IF @constraintName IS NOT NULL
    EXEC('ALTER TABLE dbo.fault_coordinates DROP CONSTRAINT ' + @constraintName);

DROP TABLE IF EXISTS [dbo].[fault_coordinates];
DROP TABLE IF EXISTS [dbo].[faultStress];
DROP TABLE IF EXISTS [dbo].[faults];
DROP TABLE IF EXISTS [dbo].[Feature];
DROP TABLE IF EXISTS [dbo].[Geometry];
DROP TABLE IF EXISTS [dbo].[Properties];