CREATE DATABASE EarthQuake;

USE EarthQuake;

-- Create the main faults table
CREATE TABLE faults (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    type NVARCHAR(50) NOT NULL,
    dip INT,
    last_movement DATE
);

-- Create the fault_coordinates table
CREATE TABLE fault_coordinates (
    id INT IDENTITY(1,1) PRIMARY KEY,
    fault_id INT NOT NULL,
    latitude FLOAT NOT NULL,
    longitude FLOAT NOT NULL,
    point_order INT NOT NULL,
    FOREIGN KEY (fault_id) REFERENCES faults(id) ON DELETE CASCADE
);

CREATE TABLE Metadata (
    MetadataId INT IDENTITY(1,1) PRIMARY KEY,
    generated BIGINT NOT NULL,
    url NVARCHAR(2083) NOT NULL,
    title NVARCHAR(MAX) NOT NULL,
    status INT NOT NULL,
    api NVARCHAR(100) NOT NULL,
    count INT NOT NULL
);

CREATE TABLE Geometry (
    GeometryId INT IDENTITY(1,1) PRIMARY KEY,
    type NVARCHAR(50) NOT NULL,
    coordinates NVARCHAR(MAX) NOT NULL -- JSON string representation of List<double>
);

CREATE TABLE Properties (
    PropertiesId INT IDENTITY(1,1) PRIMARY KEY,
    mag FLOAT NOT NULL,
    place NVARCHAR(MAX) NOT NULL,
    time FLOAT NOT NULL, -- double in C#
    updated NVARCHAR(MAX) NULL,
    tz NVARCHAR(MAX) NULL,
    url NVARCHAR(MAX) NOT NULL,
    detail NVARCHAR(MAX) NOT NULL,
    felt NVARCHAR(MAX) NULL,
    cdi NVARCHAR(MAX) NULL,
    mmi FLOAT NULL,
    alert NVARCHAR(MAX) NULL,
    status NVARCHAR(50) NOT NULL,
    tsunami INT NOT NULL,
    sig INT NOT NULL,
    net NVARCHAR(50) NOT NULL,
    code NVARCHAR(50) NOT NULL,
    ids NVARCHAR(MAX) NOT NULL,
    sources NVARCHAR(MAX) NOT NULL,
    types NVARCHAR(MAX) NOT NULL,
    nst INT NULL,
    dmin FLOAT NULL,
    rms FLOAT NOT NULL,
    gap FLOAT NULL,
    magType NVARCHAR(50) NOT NULL,
    type NVARCHAR(50) NOT NULL,
    title NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Feature (
    id NVARCHAR(100) PRIMARY KEY,
    type NVARCHAR(50) NOT NULL,
    PropertiesId INT NOT NULL,
    GeometryId INT NOT NULL,
    CONSTRAINT FK_Feature_Properties FOREIGN KEY (PropertiesId) REFERENCES Properties(PropertiesId),
    CONSTRAINT FK_Feature_Geometry FOREIGN KEY (GeometryId) REFERENCES Geometry(GeometryId)
);

CREATE TYPE Properties AS TABLE(        
    mag FLOAT NOT NULL,
    place NVARCHAR(255) NOT NULL,
    time BIGINT NOT NULL,                     
    updated NVARCHAR(100) NULL,               
    tz NVARCHAR(50) NULL,                    
    url NVARCHAR(2083) NOT NULL,
    detail NVARCHAR(2083) NOT NULL,
    felt NVARCHAR(50) NULL,                   
    cdi NVARCHAR(50) NULL,
    mmi FLOAT NULL,
    alert NVARCHAR(50) NULL,                  
    status NVARCHAR(50) NOT NULL,
    tsunami INT NOT NULL,
    sig INT NOT NULL,
    net NVARCHAR(50) NOT NULL,
    code NVARCHAR(50) NOT NULL,
    ids NVARCHAR(255) NOT NULL,
    sources NVARCHAR(255) NOT NULL,
    types NVARCHAR(255) NOT NULL,
    nst INT NULL,
    dmin FLOAT NULL,
    rms FLOAT NOT NULL,
    gap FLOAT NULL,
    magType NVARCHAR(50) NOT NULL,
    type NVARCHAR(50) NOT NULL,
    title NVARCHAR(255) NOT NULL
);

CREATE TYPE Geo AS TABLE
(
    Id INT NOT NULL, 
    type NVARCHAR(100) NOT NULL
);

CREATE TYPE Coordinates AS TABLE
(
    GeoId INT NOT NULL,       
    PointOrder INT NOT NULL,       
    Coordinate FLOAT NOT NULL      
);

CREATE TYPE Feature AS TABLE
(
    Type NVARCHAR(100) NOT NULL,
    PropertiesId INT NOT NULL,
    GeoId INT NOT NULL,
    Id NVARCHAR(100) NOT NULL
);
