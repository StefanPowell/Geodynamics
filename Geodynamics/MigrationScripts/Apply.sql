CREATE DATABASE EarthQuake;
GO

USE EarthQuake;
GO

CREATE SCHEMA quake;
GO

CREATE TABLE [dbo].[Properties](
	[PropertiesId] [int] IDENTITY(1,1) NOT NULL,
	[mag] [float] NOT NULL,
	[place] [nvarchar](max) NOT NULL,
	[time] [float] NOT NULL,
	[updated] [nvarchar](max) NULL,
	[tz] [nvarchar](max) NULL,
	[url] [nvarchar](max) NOT NULL,
	[detail] [nvarchar](max) NOT NULL,
	[felt] [nvarchar](max) NULL,
	[cdi] [nvarchar](max) NULL,
	[mmi] [float] NULL,
	[alert] [nvarchar](max) NULL,
	[status] [nvarchar](50) NOT NULL,
	[tsunami] [int] NOT NULL,
	[sig] [int] NOT NULL,
	[net] [nvarchar](50) NOT NULL,
	[code] [nvarchar](50) NOT NULL,
	[ids] [nvarchar](max) NOT NULL,
	[sources] [nvarchar](max) NOT NULL,
	[types] [nvarchar](max) NOT NULL,
	[nst] [int] NULL,
	[dmin] [float] NULL,
	[rms] [float] NOT NULL,
	[gap] [float] NULL,
	[magType] [nvarchar](50) NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PropertiesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Geometry](
	[GeometryId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Latitude] [real] NOT NULL,
	[Longitude] [real] NOT NULL,
	[Depth] [real] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GeometryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Feature](
	[FeatureId] [int] IDENTITY(1,1) NOT NULL,
	[FeatureKey] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[PropertiesId] [int] NOT NULL,
	[GeometryId] [int] NOT NULL,
 CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED 
(
	[FeatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Feature]  WITH CHECK ADD  CONSTRAINT [FK_Feature_Geometry] FOREIGN KEY([GeometryId])
REFERENCES [dbo].[Geometry] ([GeometryId])
GO

ALTER TABLE [dbo].[Feature] CHECK CONSTRAINT [FK_Feature_Geometry]
GO

ALTER TABLE [dbo].[Feature]  WITH CHECK ADD  CONSTRAINT [FK_Feature_Properties] FOREIGN KEY([PropertiesId])
REFERENCES [dbo].[Properties] ([PropertiesId])
GO

ALTER TABLE [dbo].[Feature] CHECK CONSTRAINT [FK_Feature_Properties]
GO

CREATE TABLE [dbo].[faults](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[dip] [int] NULL,
	[last_movement] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[fault_coordinates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fault_id] [int] NOT NULL,
	[latitude] [float] NOT NULL,
	[longitude] [float] NOT NULL,
	[point_order] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[fault_coordinates]  WITH CHECK ADD FOREIGN KEY([fault_id])
REFERENCES [dbo].[faults] ([id])
ON DELETE CASCADE
GO


CREATE TABLE [dbo].[faultStress](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[faultId] [int] NOT NULL,
	[faultName] [nvarchar](255) NOT NULL,
	[slipRate_m_per_yr] [float] NULL,
	[lockedThickness_m] [float] NULL,
	[ruptureLength_m] [float] NULL,
	[shearStressRate_MPa_per_yr] [float] NULL,
	[accumulatedStress_MPa] [float] NULL,
	[expectedSlip_m] [float] NULL,
	[momentMagnitude_Mw] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[faultStress]  WITH CHECK ADD  CONSTRAINT [FK_faultStress_faultId] FOREIGN KEY([faultId])
REFERENCES [dbo].[faults] ([id])
GO


CREATE TABLE [dbo].[Metadata](
	[MetadataId] [int] IDENTITY(1,1) NOT NULL,
	[generated] [bigint] NOT NULL,
	[url] [nvarchar](2083) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[status] [int] NOT NULL,
	[api] [nvarchar](100) NOT NULL,
	[count] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MetadataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--Table Types

CREATE TYPE [dbo].[Coordinates] AS TABLE(
	[GeoId] [int] NOT NULL,
	[PointOrder] [int] NOT NULL,
	[Coordinate] [float] NOT NULL
)
GO

CREATE TYPE [dbo].[FaultStressType] AS TABLE(
	[faultName] [nvarchar](255) NOT NULL,
	[slipRate_m_per_yr] [float] NULL,
	[lockedThickness_m] [float] NULL,
	[ruptureLength_m] [float] NULL,
	[shearStressRate_MPa_per_yr] [float] NULL,
	[accumulatedStress_MPa] [float] NULL,
	[expectedSlip_m] [float] NULL,
	[momentMagnitude_Mw] [float] NULL
)
GO

CREATE TYPE [dbo].[FeatureType] AS TABLE(
	[Type] [nvarchar](100) NOT NULL,
	[PropertiesId] [int] NOT NULL,
	[GeoId] [int] NOT NULL,
	[Id] [nvarchar](100) NOT NULL
)
GO

CREATE TYPE [dbo].[Geo] AS TABLE(
	[latitude] [real] NOT NULL,
	[longitude] [real] NOT NULL,
	[depth] [real] NOT NULL,
	[type] [nvarchar](100) NOT NULL
)
GO

CREATE TYPE [dbo].[Identifiers] AS TABLE(
	[Id] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](100) NOT NULL
)
GO

CREATE TYPE [dbo].[Properties] AS TABLE(
	[mag] [float] NOT NULL,
	[place] [nvarchar](255) NOT NULL,
	[time] [float] NOT NULL,
	[updated] [nvarchar](100) NULL,
	[tz] [nvarchar](50) NULL,
	[url] [nvarchar](2083) NOT NULL,
	[detail] [nvarchar](2083) NOT NULL,
	[felt] [nvarchar](50) NULL,
	[cdi] [nvarchar](50) NULL,
	[mmi] [float] NULL,
	[alert] [nvarchar](50) NULL,
	[status] [nvarchar](50) NOT NULL,
	[tsunami] [int] NOT NULL,
	[sig] [int] NOT NULL,
	[net] [nvarchar](50) NOT NULL,
	[code] [nvarchar](50) NOT NULL,
	[ids] [nvarchar](255) NOT NULL,
	[sources] [nvarchar](255) NOT NULL,
	[types] [nvarchar](255) NOT NULL,
	[nst] [int] NULL,
	[dmin] [float] NULL,
	[rms] [float] NOT NULL,
	[gap] [float] NULL,
	[magType] [nvarchar](50) NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[title] [nvarchar](255) NOT NULL
)
GO

--Stored Procedures

CREATE PROCEDURE [dbo].[GetLatestFeatures]
    @TotalValues INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TotalValues)
        f.FeatureKey AS Id,
        f.[Type] AS FeatureType,
        g.[Type] AS GeometryType,
        g.Latitude,
        g.Longitude,
        g.Depth,
        p.mag AS Magnitude,
        p.place AS Place,
        DATEADD(SECOND, p.time / 1000, '1970-01-01') AS QuakeDateTime
    FROM dbo.Geometry g
    INNER JOIN dbo.Feature f ON f.GeometryId = g.GeometryId
    INNER JOIN dbo.Properties p ON f.PropertiesId = p.PropertiesId
    ORDER BY p.time DESC; 
END;
GO

CREATE PROCEDURE [dbo].[InsertFeatures]
    @Identifiers dbo.Identifiers READONLY,
    @Geo dbo.Geo READONLY,
    @Properties dbo.Properties READONLY
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Inserted TABLE (RowNum INT IDENTITY(1,1), PropertiesId INT);
    DECLARE @InsertedGeo TABLE (RowNum INT IDENTITY(1,1), GeometryId INT);
    DECLARE @IdentifiersWithRow TABLE (RowNum INT IDENTITY(1,1), Id NVARCHAR(100), Type NVARCHAR(50));

    -- Copy identifiers with row numbers for alignment
    INSERT INTO @IdentifiersWithRow (Id, Type)
    SELECT Id, Type FROM @Identifiers;

    -- Insert into Properties
    INSERT INTO dbo.Properties
    (
        [mag], [place], [time], [updated], [tz], [url], [detail],
        [felt], [cdi], [mmi], [alert], [status], [tsunami], [sig],
        [net], [code], [ids], [sources], [types], [nst], [dmin],
        [rms], [gap], [magType], [type], [title]
    )
    OUTPUT INSERTED.PropertiesId INTO @Inserted(PropertiesId)
    SELECT 
        p.[mag], p.[place], p.[time], p.[updated], p.[tz], p.[url], p.[detail],
        p.[felt], p.[cdi], p.[mmi], p.[alert], p.[status], p.[tsunami], p.[sig],
        p.[net], p.[code], p.[ids], p.[sources], p.[types], p.[nst], p.[dmin],
        p.[rms], p.[gap], p.[magType], p.[type], p.[title]
    FROM @Properties p;

    -- Insert into Geometry
    INSERT INTO dbo.Geometry
    (
        [Type], [Latitude], [Longitude], [Depth]
    )
    OUTPUT INSERTED.GeometryId INTO @InsertedGeo(GeometryId)
    SELECT
        g.[type],
		g.[latitude],
		g.[longitude],
		g.[depth]
    FROM @Geo g;

    -- Insert into Feature (align by row number)
    INSERT INTO dbo.Feature ([FeatureKey], [Type], [PropertiesId], [GeometryId])
    SELECT 
        i.Id,
        i.Type,
        p.PropertiesId,
        g.GeometryId
    FROM @IdentifiersWithRow i
    INNER JOIN @Inserted p ON i.RowNum = p.RowNum
    INNER JOIN @InsertedGeo g ON i.RowNum = g.RowNum;
END;
GO

CREATE PROCEDURE [dbo].[sp_Feature_Get_AllWithGridAndTime]
    @latitudeStart REAL,
    @latitudeEnd REAL,
    @longitudeStart REAL,
    @longitudeEnd REAL,
    @startDate DATETIME,
    @endDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
	    f.FeatureKey AS Id,
		f.[Type] AS FeatureType,
		g.[Type] AS GeometryType,
		g.Latitude,
		g.Longitude,
		g.Depth,
        p.mag AS Magnitude,
		p.place AS Place,
		DATEADD(SECOND, p.time / 1000, '1970-01-01') AS QuakeDateTime
    FROM dbo.Geometry g
    INNER JOIN dbo.Feature f ON f.GeometryId = g.GeometryId
    INNER JOIN dbo.Properties p ON f.PropertiesId = p.PropertiesId 
    WHERE g.Latitude  BETWEEN @latitudeStart  AND @latitudeEnd
      AND g.Longitude BETWEEN @longitudeStart AND @longitudeEnd
      AND DATEADD(SECOND, p.time / 1000, '1970-01-01') 
            BETWEEN @startDate AND @endDate;
END;
GO

CREATE PROCEDURE [dbo].[UpdateFaultStress]
    @FaultStressList [dbo].[FaultStressType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO [dbo].[faultStress] AS target
    USING @FaultStressList AS source
    ON target.faultName = source.faultName
    WHEN MATCHED THEN
        UPDATE SET
            slipRate_m_per_yr = source.slipRate_m_per_yr,
            lockedThickness_m = source.lockedThickness_m,
            ruptureLength_m = source.ruptureLength_m,
            shearStressRate_MPa_per_yr = source.shearStressRate_MPa_per_yr,
            accumulatedStress_MPa = source.accumulatedStress_MPa,
            expectedSlip_m = source.expectedSlip_m,
            momentMagnitude_Mw = source.momentMagnitude_Mw
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (faultName, slipRate_m_per_yr, lockedThickness_m, ruptureLength_m, shearStressRate_MPa_per_yr, accumulatedStress_MPa, expectedSlip_m, momentMagnitude_Mw)
        VALUES (source.faultName, source.slipRate_m_per_yr, source.lockedThickness_m, source.ruptureLength_m, source.shearStressRate_MPa_per_yr, source.accumulatedStress_MPa, source.expectedSlip_m, source.momentMagnitude_Mw);
END;
GO