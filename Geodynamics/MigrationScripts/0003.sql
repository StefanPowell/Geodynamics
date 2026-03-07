CREATE TABLE [dbo].[fault](
	[id] INT IDENTITY(1,1) NOT NULL,
	[averageDip] NVARCHAR(255) NULL,
	[averageRake] NVARCHAR(50) NULL,
	[catalogId] INT NULL,
	[catalogName] NVARCHAR(100) NULL,
	[dipDir] NVARCHAR(100) NULL,
	[lowerSeisDepth] NVARCHAR(100) NULL,
	[name] NVARCHAR(255) NULL,
	[netSlipRate] NVARCHAR(100) NULL,
	[slipType] NVARCHAR(100) NULL,
	[upperSeisDepth] NVARCHAR(100) NULL,
	[geometryType] NVARCHAR(100) NULL,
	CONSTRAINT PK_faults PRIMARY KEY CLUSTERED (id)
);
GO

CREATE TABLE [dbo].[faultCoordinates](
	[id] INT IDENTITY(1,1) NOT NULL,
	[fault_id] INT NOT NULL,
	[point_order] INT NOT NULL,
	[latitude] FLOAT NOT NULL,
	[longitude] FLOAT NOT NULL,
	CONSTRAINT PK_faultCoordinates PRIMARY KEY CLUSTERED (id),
	CONSTRAINT FK_faultCoordinates_faults
		FOREIGN KEY (fault_id)
		REFERENCES [dbo].[fault](id)
		ON DELETE CASCADE
);
GO

CREATE TYPE [dbo].[FaultLine] AS TABLE(
	[averageDip] NVARCHAR(255) NULL,
	[averageRake] NVARCHAR(50) NULL,
	[catalogId] INT NULL,
	[catalogName] NVARCHAR(100) NULL,
	[dipDir] NVARCHAR(100) NULL,
	[lowerSeisDepth] NVARCHAR(100) NULL,
	[name] NVARCHAR(255) NULL,
	[netSlipRate] NVARCHAR(100) NULL,
	[slipType] NVARCHAR(100) NULL,
	[upperSeisDepth] NVARCHAR(100) NULL,
	[geometryType] NVARCHAR(100) NULL,
	[co_ordinateList] VARCHAR(MAX) NOT NULL -- [(1, 18, -77), (2, 19, -76)]
);
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
REFERENCES [dbo].[fault] ([id])
GO


CREATE PROCEDURE [dbo].[InsertFaultLines]
    @FaultLine [dbo].[FaultLine] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdMap TABLE (fault_id INT, coord_string NVARCHAR(MAX));

    MERGE INTO dbo.fault AS Target
    USING @FaultLine AS Source
    ON 1 = 0 -- Always false to force an INSERT
    WHEN NOT MATCHED THEN
        INSERT ([averageDip], [averageRake], [catalogId], [catalogName], [dipDir], 
                [lowerSeisDepth], [name], [netSlipRate], [slipType], [upperSeisDepth], [geometryType])
        VALUES (Source.[averageDip], Source.[averageRake], Source.[catalogId], Source.[catalogName], Source.[dipDir], 
                Source.[lowerSeisDepth], Source.[name], Source.[netSlipRate], Source.[slipType], Source.[upperSeisDepth], Source.[geometryType])
    OUTPUT INSERTED.id, Source.[co_ordinateList] INTO @IdMap(fault_id, coord_string);

    -- 3. Process all coordinates for all inserted rows in one set-based operation
    INSERT INTO [dbo].[faultCoordinates] (fault_id, point_order, latitude, longitude)
    SELECT 
        m.fault_id,
        CAST(JSON_VALUE(ca.value, '$[0]') AS INT) AS point_order,
        CAST(JSON_VALUE(ca.value, '$[1]') AS FLOAT) AS latitude,
        CAST(JSON_VALUE(ca.value, '$[2]') AS FLOAT) AS longitude
    FROM @IdMap m
    CROSS APPLY (
        SELECT '[' + REPLACE(REPLACE(REPLACE(REPLACE(m.coord_string, '[', ''), ']', ''), '(', '['), ')', ']') + ']' AS json_formatted
    ) clean
    CROSS APPLY OPENJSON(clean.json_formatted) ca; 
END;
GO