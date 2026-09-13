CREATE PROCEDURE usp_GetClosestEarthquake_ByMiles
(
    @Latitude FLOAT,
    @Longitude FLOAT,
    @Miles INT
)
AS BEGIN
    SET NOCOUNT ON;

    WITH CalculatedDistances AS (
        SELECT
            F.FeatureId,
            G.Latitude AS Latitude2,
            G.Longitude AS Longitude2,
            ROUND(3958.8 * ACOS(COS(@LATITUDE * 0.017453) * COS(G.Latitude * 0.017453) * COS((@LONGITUDE - G.Longitude) * 0.017453) + SIN(@LATITUDE * 0.017453) * SIN(G.Latitude * 0.017453)), 2) AS miles
        FROM
            dbo.Feature F
            JOIN dbo.[Geometry] G ON F.GeometryId = G.GeometryId
    )
    SELECT * 
    FROM CalculatedDistances
    WHERE miles < @Miles AND miles > 0
    ORDER BY miles ASC;
END;
Go


CREATE PROCEDURE usp_Feature_Delete_QuakesNeedingWaveform
(
    @FeatureId INT
)
AS
BEGIN
	DELETE FROM [EarthQuake].[dbo].[PendingWaveformDataRetrieval] 
    WHERE FeatureId = @FeatureId;
END;
Go

CREATE TABLE [dbo].[GroundStation]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Network] NVARCHAR(255) NOT NULL,
    [Station] NVARCHAR(255) NOT NULL,
    [Latitude] FLOAT NOT NULL,
    [Longitude] FLOAT NOT NULL,
    [Elevation] FLOAT NOT NULL,
    [SiteName] NVARCHAR(255) NOT NULL,
    [StartTime] DATETIME NOT NULL,
    [EndTime] DATETIME NULL
);

CREATE TABLE [dbo].[WaveformInfo]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FeatureId] INT NOT NULL,
    [GroundStationId] INT NOT NULL,
    FOREIGN KEY (FeatureId) REFERENCES Feature(FeatureId)
);
Go

CREATE PROCEDURE dbo.usp_Set_FeatureAssociated_GroundStation
(
    @FeatureId INT,
    @Network VARCHAR(50),
    @Station VARCHAR(50),
    @Latitude FLOAT,
    @Longitude FLOAT,
    @Elevation FLOAT,
    @SiteName VARCHAR(255),
    @StartTime DATETIME,
    @EndTime DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InsertedGroundStationId INT;

    INSERT INTO dbo.GroundStation
    (
        Network,
        Station,
        Latitude,
        Longitude,
        Elevation,
        SiteName,
        StartTime,
        EndTime
    )
    VALUES
    (
        @Network,
        @Station,
        @Latitude,
        @Longitude,
        @Elevation,
        @SiteName,
        @StartTime,
        @EndTime
    );

    SET @InsertedGroundStationId = CONVERT(INT, SCOPE_IDENTITY());

    INSERT INTO dbo.WaveformInfo
    (
        FeatureId,
        GroundStationId
    )
    VALUES
    (
        @FeatureId,
        @InsertedGroundStationId
    );
END;
GO