CREATE TABLE [dbo].[world_stress_map] (
    ID VARCHAR(20) PRIMARY KEY,
    ISC_ID VARCHAR(20) NULL,
    SITE VARCHAR(20) NULL,
    LAT DECIMAL(8,5) NULL,
    LON DECIMAL(8,5) NULL,
    AZI DECIMAL(6,2) NULL,
    TYPE VARCHAR(10) NULL,
    DEPTH DECIMAL(6,2) NULL,
    QUALITY CHAR(1) NULL,
    REGIME VARCHAR(5) NULL,
    LOCALITY VARCHAR(100) NULL,
    COUNTRY VARCHAR(100) NULL,
    DATE DATE NULL,
    TIME TIME NULL,
    NUMBER INT NULL,
    SD DECIMAL(6,2) NULL,
    TOT_LEN DECIMAL(10,2) NULL,
    VENT VARCHAR(20) NULL,
    [TOP] DECIMAL(10,2) NULL,
    BOT DECIMAL(10,2) NULL,
    ANISOTROPY VARCHAR(10) NULL,
    METHOD VARCHAR(10) NULL,
    S1AZ DECIMAL(6,2) NULL,
    S1PL DECIMAL(6,2) NULL,
    S2AZ DECIMAL(6,2) NULL,
    S2PL DECIMAL(6,2) NULL,
    S3AZ DECIMAL(6,2) NULL,
    S3PL DECIMAL(6,2) NULL,
    MAG_TYPE VARCHAR(10) NULL,
    EQ_MAG DECIMAL(4,2) NULL,
    CRUST VARCHAR(20) NULL,
    REF1 VARCHAR(50) NULL,
    REF2 VARCHAR(50) NULL,
    REF3 VARCHAR(50) NULL,
    REF4 VARCHAR(50) NULL,
    REF5 VARCHAR(50) NULL,
    REF6 VARCHAR(50) NULL,
    COMMENT TEXT NULL,
    PLATE VARCHAR(10) NULL,
    DIST DECIMAL(10,2) NULL
);

CREATE TYPE [dbo].[CrustStress] AS TABLE(
	[id] [nvarchar](20) NOT NULL,
    [isc_id] [nvarchar](20) NULL,
    [site] [nvarchar](20) NULL,
    [lat] decimal(8,5) NULL,
    [lon] decimal(8,5) NULL,
    [azi] decimal(6,2) NULL,
    [type] [nvarchar](10) NULL,
    [depth] decimal(6,2) NULL,
    [quality] char(1) NULL,
    [regime] [nvarchar](5) NULL,
    [locality] [nvarchar](100) NULL,
    [country] [nvarchar](100) NULL,
    [date] DATE NULL,
    [time] TIME NULL,
    [number] INT NULL,
    [sd] decimal(6,2) NULL,
    [tot_len] decimal(10,2) NULL,
    [vent] [nvarchar](20) NULL,
    [top] decimal(10,2) NULL,
    [bot] decimal(10,2) NULL,
    [anisotropy] [nvarchar](10) NULL,
    [method] [nvarchar](10) NULL,
    [s1az] decimal(6,2) NULL,
    [s1pl] decimal(6,2) NULL,
    [s2az] decimal(6,2) NULL,
    [s2pl] decimal(6,2) NULL,
    [s3az] decimal(6,2) NULL,
    [s3pl] decimal(6,2) NULL,
    [mag_type] [nvarchar](10) NULL,
    [eq_mag] decimal(4,2) NULL,
    [crust] [nvarchar](20) NULL,
    [ref1] [nvarchar](20) NULL,
    [ref2] [nvarchar](20) NULL,
    [ref3] [nvarchar](20) NULL,
    [ref4] [nvarchar](20) NULL,
    [ref5] [nvarchar](20) NULL,
    [ref6] [nvarchar](20) NULL,
    [comment] nvarchar(max) NULL,
    [plate] [nvarchar](10) NULL,
    [dist] decimal(10,2) NULL
)
GO

CREATE PROCEDURE [dbo].[InsertCrustStress]
    @CrustStress [dbo].[CrustStress] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[world_stress_map] (
    ID, ISC_ID, SITE, LAT, LON, AZI, TYPE, DEPTH, QUALITY, REGIME,
    LOCALITY, COUNTRY, DATE, TIME, NUMBER, SD, TOT_LEN, VENT, [TOP], BOT,
    ANISOTROPY, METHOD, S1AZ, S1PL, S2AZ, S2PL, S3AZ, S3PL,
    MAG_TYPE, EQ_MAG, CRUST,
    REF1, REF2, REF3, REF4, REF5, REF6,
    COMMENT, PLATE, DIST
    )
    SELECT
        id, isc_id, site, lat, lon, azi, type, depth, quality, regime,
        locality, country, date, time, number, sd, tot_len, vent, [top], bot,
        anisotropy, method, s1az, s1pl, s2az, s2pl, s3az, s3pl,
        mag_type, eq_mag, crust,
        ref1, ref2, ref3, ref4, ref5, ref6,
        comment, plate, dist
    FROM @CrustStress;

END;
GO