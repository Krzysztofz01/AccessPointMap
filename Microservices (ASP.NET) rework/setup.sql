CREATE DATABASE AccessPointMap;
GO
USE AccessPointMap;

CREATE TABLE Accesspoints (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Bssid] VARCHAR(25) NOT NULL UNIQUE,
    [Ssid] VARCHAR(50) NOT NULL,
    [Frequency] INT NOT NULL,
    [HighSignalLevel] INT NOT NULL,
    [HighLongitude] FLOAT NOT NULL,
    [HighLatitude] FLOAT NOT NULL,
    [LowSignalLevel] INT NOT NULL,
    [LowLongitude] FLOAT NOT NULL,
    [LowLatitude] FLOAT NOT NULL,
    [SignalRadius] FLOAT NOT NULL,
    [SignalArea] FLOAT NOT NULL,
    [SecurityData] VARCHAR(65) NOT NULL,
    [Brand] VARCHAR(60) DEFAULT 'No brand info',
    [DeviceType] VARCHAR(50) NOT NULL,
    [Display] BIT DEFAULT 1,
    [PostedBy] VARCHAR(60) DEFAULT 'Admin',
    [CreateDate] DATETIME2(6) DEFAULT GETDATE(),
    [UpdateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE GuestAccesspoints (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Bssid] VARCHAR(25) NOT NULL UNIQUE,
    [Ssid] VARCHAR(50) NOT NULL,
    [Frequency] INT NOT NULL,
    [HighSignalLevel] INT NOT NULL,
    [HighLongitude] FLOAT NOT NULL,
    [HighLatitude] FLOAT NOT NULL,
    [LowSignalLevel] INT NOT NULL,
    [LowLongitude] FLOAT NOT NULL,
    [LowLatitude] FLOAT NOT NULL,
    [SignalRadius] FLOAT NOT NULL,
    [SignalArea] FLOAT NOT NULL,
    [SecurityData] VARCHAR(65) NOT NULL,
    [DeviceType] VARCHAR(50) NOT NULL,
    [PostedBy] VARCHAR(60) NOT NULL,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE Users (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Login] VARCHAR(50) NOT NULL UNIQUE,
    [Password] VARCHAR(60) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [Token] CHAR NOT NULL,
    [TokenExpiration] INT NOT NULL,
    [WritePermission] BIT NOT NULL,
    [ReadPermission] BIT NOT NULL,
    [AdminPermission] BIT NOT NULL,
    [CreateDate] DATETIME2(6) DEFAULT GETDATE(),
    [LastLoginDate] DATETIME2(6) DEFAULT GETDATE(),
    [LastLoginIp] VARCHAR(45) NOT NULL,
    [Active] BIT DEFAULT 0
);

GO

CREATE TABLE Logs (
    [Id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Status] VARCHAR(10) NOT NULL,
    [Endpoint] VARCHAR(35) NOT NULL,
    [Description] VARCHAR(150) NOT NULL,
    [EventDate] DATETIME2(6) DEFAULT GETDATE(),
);

GO