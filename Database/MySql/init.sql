CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

CREATE TABLE `Users` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `AddDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `EditDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `DeleteDate` datetime(6) NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(255) NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `LastLoginIp` longtext CHARACTER SET utf8mb4 NOT NULL,
    `LastLoginDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `AdminPermission` tinyint(1) NOT NULL DEFAULT FALSE,
    `ModPermission` tinyint(1) NOT NULL DEFAULT FALSE,
    `IsActivated` tinyint(1) NOT NULL DEFAULT FALSE,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
);

CREATE TABLE `AccessPoints` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `AddDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `EditDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `DeleteDate` datetime(6) NULL,
    `Bssid` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Ssid` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Fingerprint` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Frequency` double NOT NULL,
    `MaxSignalLevel` int NOT NULL,
    `MaxSignalLongitude` double NOT NULL,
    `MaxSignalLatitude` double NOT NULL,
    `MinSignalLevel` int NOT NULL,
    `MinSignalLongitude` double NOT NULL,
    `MinSignalLatitude` double NOT NULL,
    `SignalRadius` double NOT NULL,
    `SignalArea` double NOT NULL,
    `FullSecurityData` longtext CHARACTER SET utf8mb4 NOT NULL,
    `SerializedSecurityData` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IsSecure` tinyint(1) NOT NULL DEFAULT FALSE,
    `IsHidden` tinyint(1) NOT NULL DEFAULT FALSE,
    `Manufacturer` longtext CHARACTER SET utf8mb4 NULL,
    `DeviceType` longtext CHARACTER SET utf8mb4 NULL,
    `MasterGroup` tinyint(1) NOT NULL DEFAULT FALSE,
    `Display` tinyint(1) NOT NULL DEFAULT FALSE,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `UserAddedId` bigint NULL,
    `UserModifiedId` bigint NULL,
    CONSTRAINT `PK_AccessPoints` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AccessPoints_Users_UserAddedId` FOREIGN KEY (`UserAddedId`) REFERENCES `Users` (`Id`),
    CONSTRAINT `FK_AccessPoints_Users_UserModifiedId` FOREIGN KEY (`UserModifiedId`) REFERENCES `Users` (`Id`)
);

CREATE TABLE `RefreshTokens` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `AddDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `EditDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `DeleteDate` datetime(6) NULL,
    `Token` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Expires` datetime(6) NOT NULL,
    `Created` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `CreatedByIp` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Revoked` datetime(6) NULL,
    `RevokedByIp` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IsRevoked` tinyint(1) NOT NULL DEFAULT FALSE,
    `ReplacedByToken` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` bigint NULL,
    CONSTRAINT `PK_RefreshTokens` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RefreshTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE SET NULL
);

CREATE INDEX `IX_AccessPoints_UserAddedId` ON `AccessPoints` (`UserAddedId`);

CREATE INDEX `IX_AccessPoints_UserModifiedId` ON `AccessPoints` (`UserModifiedId`);

CREATE INDEX `IX_RefreshTokens_UserId` ON `RefreshTokens` (`UserId`);

CREATE UNIQUE INDEX `IX_Users_Email` ON `Users` (`Email`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210724131531_Init', '5.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `Users` MODIFY COLUMN `LastLoginIp` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `RevokedByIp` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `CreatedByIp` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210726110001_StringDefaultValues', '5.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `Users` DROP COLUMN `DeleteDate`;

ALTER TABLE `RefreshTokens` DROP COLUMN `DeleteDate`;

ALTER TABLE `AccessPoints` DROP COLUMN `DeleteDate`;

ALTER TABLE `Users` MODIFY COLUMN `Password` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `Name` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `LastLoginIp` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `Users` MODIFY COLUMN `EditDate` datetime(6) NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `AddDate` datetime(6) NOT NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `Token` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `RevokedByIp` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `ReplacedByToken` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `EditDate` datetime(6) NOT NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `CreatedByIp` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `RefreshTokens` MODIFY COLUMN `AddDate` datetime(6) NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `Ssid` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `SerializedSecurityData` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `Note` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `Manufacturer` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `FullSecurityData` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `Fingerprint` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `EditDate` datetime(6) NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `DeviceType` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `Bssid` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `AccessPoints` MODIFY COLUMN `AddDate` datetime(6) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210822155716_DomainModelCleanup', '5.0.9');

COMMIT;

START TRANSACTION;

ALTER TABLE `Users` ADD `DeleteDate` datetime(6) NULL;

ALTER TABLE `RefreshTokens` ADD `DeleteDate` datetime(6) NULL;

ALTER TABLE `AccessPoints` ADD `DeleteDate` datetime(6) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210823100745_DeleteDateFieldFix', '5.0.9');

COMMIT;

START TRANSACTION;

INSERT INTO `Users` (`Id`, `AddDate`, `AdminPermission`, `DeleteDate`, `EditDate`, `Email`, `IsActivated`, `LastLoginDate`, `LastLoginIp`, `Name`, `Password`)
VALUES (1, '2021-08-24 14:45:46', TRUE, NULL, '2021-08-24 14:45:46', 'admin@apm.com', TRUE, '2021-08-24 14:45:46', '', 'Administrator', '$05$feN415S/rRMOaPcaiobkEeo5JTPoxY7PPMCwVGkbrbItw/mj19CBS');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210824124547_DefaultAdminUser', '5.0.9');

COMMIT;

