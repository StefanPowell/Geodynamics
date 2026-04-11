CREATE TABLE [dbo].[world_stress_map] (
    ID VARCHAR(MAX) NULL,
    ISC_ID VARCHAR(MAX) NULL,
    [SITE] VARCHAR(MAX) NULL,
    LAT DECIMAL(8,5) NULL,
    LON DECIMAL(8,5) NULL,
    AZI DECIMAL(6,2) NULL,
    [TYPE] VARCHAR(MAX) NULL,
    DEPTH DECIMAL(6,2) NULL,
    QUALITY VARCHAR(MAX) NULL,
    REGIME VARCHAR(MAX) NULL,
    LOCALITY VARCHAR(MAX) NULL,
    COUNTRY VARCHAR(MAX) NULL,
    [DATE] VARCHAR(MAX) NULL,
    [TIME] VARCHAR(MAX) NULL,
    NUMBER INT NULL,
    SD DECIMAL(6,2) NULL,
    TOT_LEN DECIMAL(10,2) NULL,
    VENT VARCHAR(MAX) NULL,
    [TOP] DECIMAL(10,2) NULL,
    BOT DECIMAL(10,2) NULL,
    ANISOTROPY VARCHAR(MAX) NULL,
    METHOD VARCHAR(MAX) NULL,
    S1AZ DECIMAL(6,2) NULL,
    S1PL DECIMAL(6,2) NULL,
    S2AZ DECIMAL(6,2) NULL,
    S2PL DECIMAL(6,2) NULL,
    S3AZ DECIMAL(6,2) NULL,
    S3PL VARCHAR(MAX) NULL,
    MAG_TYPE VARCHAR(MAX) NULL,
    EQ_MAG VARCHAR(MAX) NULL,
    CRUST VARCHAR(MAX) NULL,
    REF1 VARCHAR(MAX) NULL,
    REF2 VARCHAR(MAX) NULL,
    REF3 VARCHAR(MAX) NULL,
    REF4 VARCHAR(MAX) NULL,
    REF5 VARCHAR(MAX) NULL,
    REF6 VARCHAR(MAX) NULL,
    COMMENT VARCHAR(MAX) NULL,
    PLATE VARCHAR(MAX) NULL,
    DIST DECIMAL(10,2) NULL
);
GO

CREATE TYPE [dbo].[CrustStress] AS TABLE(
    [id] VARCHAR(MAX) NOT NULL,
    [isc_id] VARCHAR(MAX) NULL,
    [site] VARCHAR(MAX) NULL,
    [lat] DECIMAL(8,5) NULL,
    [lon] DECIMAL(8,5) NULL,
    [azi] DECIMAL(6,2) NULL,
    [type] VARCHAR(MAX) NULL,
    [depth] DECIMAL(6,2) NULL,
    [quality] VARCHAR(MAX) NULL,
    [regime] VARCHAR(MAX) NULL,
    [locality] VARCHAR(MAX) NULL,
    [country] VARCHAR(MAX) NULL,
    [date] VARCHAR(MAX) NULL,
    [time] VARCHAR(MAX) NULL,
    [number] INT NULL,
    [sd] DECIMAL(6,2) NULL,
    [tot_len] DECIMAL(10,2) NULL,
    [vent] VARCHAR(MAX) NULL,
    [top] DECIMAL(10,2) NULL,
    [bot] DECIMAL(10,2) NULL,
    [anisotropy] VARCHAR(MAX) NULL,
    [method] VARCHAR(MAX) NULL,
    [s1az] DECIMAL(6,2) NULL,
    [s1pl] DECIMAL(6,2) NULL,
    [s2az] DECIMAL(6,2) NULL,
    [s2pl] DECIMAL(6,2) NULL,
    [s3az] DECIMAL(6,2) NULL,
    [s3pl] VARCHAR(MAX) NULL,
    [mag_type] VARCHAR(MAX) NULL,
    [eq_mag] VARCHAR(MAX) NULL,
    [crust] VARCHAR(MAX) NULL,
    [ref1] VARCHAR(MAX) NULL,
    [ref2] VARCHAR(MAX) NULL,
    [ref3] VARCHAR(MAX) NULL,
    [ref4] VARCHAR(MAX) NULL,
    [ref5] VARCHAR(MAX) NULL,
    [ref6] VARCHAR(MAX) NULL,
    [comment] VARCHAR(MAX) NULL,
    [plate] VARCHAR(MAX) NULL,
    [dist] DECIMAL(10,2) NULL
);
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