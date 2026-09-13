ALTER PROCEDURE  [dbo].[usp_Get_Earthquake]
(
    @FeatureId INT
)
AS BEGIN
    SET NOCOUNT ON;

    SELECT 
        --FEATURE
        Feature.[Type] AS [type],
        Feature.FeatureId AS id,
        --PROPERTIES
        FeatureProperty.mag,
        FeatureProperty.place,
        FeatureProperty.time,
        FeatureProperty.updated,
        FeatureProperty.tz,
        FeatureProperty.url,
        FeatureProperty.detail,
        FeatureProperty.felt,
        FeatureProperty.cdi,
        FeatureProperty.mmi,
        FeatureProperty.alert,
        FeatureProperty.status,
        FeatureProperty.tsunami,
        FeatureProperty.sig,
        FeatureProperty.net,
        FeatureProperty.code,
        FeatureProperty.ids,
        FeatureProperty.sources,
        FeatureProperty.types,
        FeatureProperty.nst,
        FeatureProperty.dmin,
        FeatureProperty.rms,
        FeatureProperty.gap,
        FeatureProperty.magType,
        FeatureProperty.type,
        FeatureProperty.title,
        --GEOMETRY
        FeatureGeometry.type,
        FeatureGeometry.coordinates
        FROM [dbo].[Feature] Feature
        JOIN [dbo].[Properties] FeatureProperty ON Feature.PropertiesId = FeatureProperty.PropertiesId
        JOIN [dbo].[Geometry] FeatureGeometry ON Feature.GeometryId = FeatureGeometry.GeometryId
        WHERE Feature.FeatureId = @FeatureId
END;
