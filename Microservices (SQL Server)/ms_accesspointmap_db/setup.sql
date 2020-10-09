CREATE DATABASE AccessPointMap;
GO
USE AccessPointMap;

CREATE TABLE accesspoints (
    [id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [bssid] VARCHAR(25) NOT NULL UNIQUE,
    [ssid] VARCHAR(50) NOT NULL,
    [frequency] INT NOT NULL,
    [highSignalLevel] INT NOT NULL,
    [highLongitude] FLOAT NOT NULL,
    [highLatitude] FLOAT NOT NULL,
    [lowSignalLevel] INT NOT NULL,
    [lowLongitude] FLOAT NOT NULL,
    [lowLatitude] FLOAT NOT NULL,
    [signalRadius] FLOAT NOT NULL,
    [signalArea] FLOAT NOT NULL,
    [securityData] VARCHAR(65) NOT NULL,
    [brand] VARCHAR(60) DEFAULT 'No brand info',
    [deviceType] VARCHAR(50) NOT NULL,
    [display] BIT DEFAULT 1,
    [createDate] DATETIME2(6) DEFAULT GETDATE(),
    [updateDate] DATETIME2(6) DEFAULT GETDATE()
);

GO

CREATE TABLE users (
    [id] int NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [login] VARCHAR(50) NOT NULL UNIQUE,
    [password] VARCHAR(60) NOT NULL,
    [tokenExpiration] INT NOT NULL,
    [writePermission] BIT NOT NULL,
    [readPermission] BIT NOT NULL,
    [createDate] DATETIME2(6) DEFAULT GETDATE(),
    [lastLoginDate] DATETIME2(6) DEFAULT GETDATE(),
    [lastLoginIp] VARCHAR(45) NOT NULL,
    [active] BIT DEFAULT 1
);

GO

INSERT INTO users (login, password, tokenExpiration, writePermission, readPermission, lastLoginIp)
VALUES ('root', '', 5, 1, 1, '::1');

GO