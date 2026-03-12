START TRANSACTION;

CREATE TABLE `brand` (
    `BrandId` int NOT NULL AUTO_INCREMENT,
    `BrandName` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_brand` PRIMARY KEY (`BrandId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `cartitem` (
    `CartItemId` int NOT NULL AUTO_INCREMENT,
    `SessionId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UserId` int NULL,
    `ProductId` int NOT NULL,
    `ProductName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    `OriginalPrice` decimal(65,30) NOT NULL,
    `Quantity` int NOT NULL,
    `ImageUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
    `BrandName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_cartitem` PRIMARY KEY (`CartItemId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `category` (
    `CategoryId` int NOT NULL AUTO_INCREMENT,
    `CategoryName` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_category` PRIMARY KEY (`CategoryId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `cpudetails` (
    `ProductID` int NOT NULL AUTO_INCREMENT,
    `Cores` int NULL,
    `Threads` int NULL,
    `BaseClockGHz` double NULL,
    `BoostClockGHz` double NULL,
    `Socket` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_cpudetails` PRIMARY KEY (`ProductID`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `inventory` (
    `ProductID` int NOT NULL AUTO_INCREMENT,
    `Quantity` int NOT NULL,
    `Price` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_inventory` PRIMARY KEY (`ProductID`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ramdetails` (
    `ProductID` int NOT NULL AUTO_INCREMENT,
    `CapacityGB` int NOT NULL,
    `SpeedMHz` int NOT NULL,
    `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ramdetails` PRIMARY KEY (`ProductID`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ssddetails` (
    `ProductID` int NOT NULL AUTO_INCREMENT,
    `CapacityGB` int NOT NULL,
    `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Interface` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_ssddetails` PRIMARY KEY (`ProductID`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `vgadetails` (
    `ProductID` int NOT NULL AUTO_INCREMENT,
    `MemoryGB` int NOT NULL,
    `Chipet` longtext CHARACTER SET utf8mb4 NULL,
    `Interface` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_vgadetails` PRIMARY KEY (`ProductID`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251213200447_AddMissingTables', '8.0.13');

COMMIT;

