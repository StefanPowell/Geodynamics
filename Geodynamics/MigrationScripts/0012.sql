ALTER PROCEDURE [dbo].[usp_Get_Earthquake]
(
    @FeatureId INT
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
    FROM [dbo].[Feature] Feature
    JOIN [dbo].[Geometry] FeatureGeometry ON Feature.GeometryId = FeatureGeometry.GeometryId
    JOIN [dbo].[Properties] FeatureProperty ON Feature.PropertiesId = FeatureProperty.PropertiesId
    WHERE Feature.FeatureId = @FeatureId
END;
