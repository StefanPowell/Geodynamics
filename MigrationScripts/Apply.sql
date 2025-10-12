CREATE DATABASE EarthQuake;
GO

USE EarthQuake;
GO

CREATE SCHEMA quake;
GO

USE [EarthQuake]
GO

/****** Object:  Table [dbo].[Feature]    ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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


/****** Object:  Table [dbo].[Geometry]    ******/
CREATE TABLE [dbo].[Geometry](
	[GeometryId] [int] IDENTITY(1,1) NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[coordinates] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GeometryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Properties]  ******/
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

/****** Object:  UserDefinedTableType [dbo].[Geo]    Script Date: 10/11/2025 11:21:24 PM ******/
CREATE TYPE [dbo].[Geo] AS TABLE(
	[latitude] [real] NOT NULL,
	[longitude] [real] NOT NULL,
	[depth] [real] NOT NULL,
	[type] [nvarchar](100) NOT NULL
)
GO

/****** Object:  UserDefinedTableType [dbo].[Identifiers]    Script Date: 10/11/2025 11:21:45 PM ******/
CREATE TYPE [dbo].[Identifiers] AS TABLE(
	[Id] [nvarchar](100) NOT NULL,
	[Type] [nvarchar](100) NOT NULL
)
GO

/****** Object:  UserDefinedTableType [dbo].[Properties]    Script Date: 10/11/2025 11:21:59 PM ******/
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

-- stored procedure [InsertFeatures] --

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
        [type], [coordinates]
    )
    OUTPUT INSERTED.GeometryId INTO @InsertedGeo(GeometryId)
    SELECT
        g.[type],
        CONCAT(g.latitude, ',', g.longitude, ',', g.depth)
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