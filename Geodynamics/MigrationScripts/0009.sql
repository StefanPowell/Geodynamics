--get the values for each of the stuff for the day

CREATE PROCEDURE [dbo].[usp_Get_Earthquakes_InDay]
(
    @STARTDATETIME DATETIME,
    @ENDDATETIME DATETIME
)
AS BEGIN
        SET NOCOUNT ON;

        SELECT
            Feature.FeatureId AS Id,
            Feature.[Type] AS FeatureType,
            FeatureGeometry.[Type] AS GeometryType,
            FeatureGeometry.Latitude AS Latititude,
            FeatureGeometry.Longitude AS Longitude,
            FeatureGeometry.Depth AS Depth,
            FeatureProperty.mag AS Magnitude,
            FeatureProperty.place AS Place,
            DATEADD(SECOND,  FeatureProperty.[time]/1000, '1970-01-01') AS QuakeDateTime
        FROM [dbo].[Properties] FeatureProperty
        JOIN [dbo].[Feature] Feature ON FeatureProperty.PropertiesId = Feature.PropertiesId
        JOIN [dbo].[Geometry] FeatureGeometry ON Feature.GeometryId = FeatureGeometry.GeometryId
        WHERE DATEADD(SECOND, CAST(FeatureProperty.[time] AS BIGINT) / 1000, '1970-01-01') >= @STARTDATETIME 
        AND DATEADD(SECOND, CAST(FeatureProperty.[time] AS BIGINT) / 1000, '1970-01-01') <= @ENDDATETIME;
END;
Go

