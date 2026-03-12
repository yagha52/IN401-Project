-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 13, 2025 at 09:34 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `hardwarestore`
--

-- --------------------------------------------------------

--
-- Table structure for table `brand`
--

DROP TABLE IF EXISTS `brand`;
CREATE TABLE `brand` (
  `BrandID` int(11) NOT NULL,
  `BrandName` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `brand`
--

INSERT INTO `brand` (`BrandID`, `BrandName`) VALUES
(1, 'Intel'),
(2, 'AMD'),
(3, 'Kingston'),
(4, 'Corsair'),
(5, 'Samsung'),
(6, 'NVIDIA'),
(7, 'MSI');

-- --------------------------------------------------------

--
-- Table structure for table `cartitem`
--

DROP TABLE IF EXISTS `cartitem`;
CREATE TABLE `cartitem` (
  `CartItemId` int(11) NOT NULL,
  `CustomerId` int(11) NOT NULL,
  `ProductId` int(11) NOT NULL,
  `ProductName` varchar(255) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `OriginalPrice` decimal(10,2) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `ImageUrl` varchar(255) DEFAULT NULL,
  `BrandName` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `CategoryID` int(11) NOT NULL,
  `CategoryName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `category`
--

INSERT INTO `category` (`CategoryID`, `CategoryName`) VALUES
(1, 'RAM'),
(2, 'CPU'),
(3, 'SSD'),
(4, 'VGA');

-- --------------------------------------------------------

--
-- Table structure for table `cpudetails`
--

DROP TABLE IF EXISTS `cpudetails`;
CREATE TABLE `cpudetails` (
  `ProductID` int(11) NOT NULL,
  `Cores` int(11) DEFAULT NULL,
  `Threads` int(11) DEFAULT NULL,
  `BaseClockGHz` decimal(4,2) DEFAULT NULL,
  `BoostClockGHz` decimal(4,2) DEFAULT NULL,
  `Socket` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `cpudetails`
--

INSERT INTO `cpudetails` (`ProductID`, `Cores`, `Threads`, `BaseClockGHz`, `BoostClockGHz`, `Socket`) VALUES
(6, 6, 12, 2.50, 4.40, 'LGA1700'),
(7, 6, 12, 3.70, 4.60, 'AM4'),
(8, 12, 20, 3.60, 5.00, 'LGA1700'),
(9, 8, 16, 3.80, 4.70, 'AM4'),
(10, 4, 8, 3.30, 4.30, 'LGA1700');

-- --------------------------------------------------------

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer` (
  `CustomerId` int(11) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `StreetAddress` varchar(200) DEFAULT NULL,
  `City` varchar(100) DEFAULT NULL,
  `State` varchar(50) DEFAULT NULL,
  `ZipCode` varchar(20) DEFAULT NULL,
  `Country` varchar(100) DEFAULT NULL,
  `OrderID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customer`
--

INSERT INTO `customer` (`CustomerId`, `FirstName`, `LastName`, `Email`, `Phone`, `PasswordHash`, `StreetAddress`, `City`, `State`, `ZipCode`, `Country`, `OrderID`) VALUES
(1, 'Emma', 'Williams', 'emma.williams@example.com', '(212) 555-1234', 'hashed_password_1', '742 Evergreen Terrace', 'Springfield', 'IL', NULL, NULL, NULL),
(2, 'Liam', 'Johnson', 'liam.johnson@example.com', '(310) 555-5678', 'hashed_password_2', '1600 Pennsylvania Ave', 'Washington', 'DC', NULL, NULL, NULL),
(3, 'Olivia', 'Brown', 'olivia.brown@example.com', '(415) 555-9012', 'hashed_password_3', '1 Infinite Loop', 'Cupertino', 'CA', NULL, NULL, NULL),
(4, 'Maya', 'Arrouk', 'mayaarrouk@gmail.com', '76788056', 'X0eshOFf6/f97LcXQRTVIi610d9pHc5yw1f+OdcujRI=', 'mezher', 'antelias', 'maten', '00000', 'lebanon', NULL),
(5, 'Yara', 'Eslim', 'eslimyara@gmail.com', NULL, 'XpdV9ZaPt5xrP2WcKkUEmKYlhcoFZWrpbpPZky17ntU=', NULL, NULL, NULL, NULL, NULL, NULL),
(7, 'reine', 'nahas', 'reine.nahas@gmail.com', '76737123', 'S0hx1UDVlXVQBbDLvY1+y8tH4t2iJaFmZ9bXST+h5Vc=', 'zalka', 'zalka', 'maten', '00000', 'LEBANON', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `ProductID` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Price` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
  `OrderID` int(11) NOT NULL,
  `CustomerID` int(11) DEFAULT NULL,
  `OrderDate` datetime DEFAULT current_timestamp(),
  `TotalAmount` decimal(18,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `orderitem`
--

DROP TABLE IF EXISTS `orderitem`;
CREATE TABLE `orderitem` (
  `OrderItemID` int(11) NOT NULL,
  `OrderID` int(11) DEFAULT NULL,
  `ProductID` int(11) DEFAULT NULL,
  `Quantity` int(11) DEFAULT NULL,
  `Price` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `ProductID` int(11) NOT NULL,
  `ProductName` varchar(100) NOT NULL,
  `CategoryID` int(11) NOT NULL,
  `BrandID` int(11) NOT NULL,
  `OriginalPrice` float DEFAULT NULL,
  `Price` float DEFAULT NULL,
  `image_url` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`ProductID`, `ProductName`, `CategoryID`, `BrandID`, `OriginalPrice`, `Price`, `image_url`) VALUES
(1, 'Kingston HyperX Fury 16GB DDR4 3200MHz', 1, 3, 49.99, 49.99, '/images/products/KingstonHyperXFury16GBDDR43200MHz.jpg'),
(2, 'Corsair Vengeance LPX 8GB DDR4 3000MHz', 1, 4, 39.5, 39.5, '/images/products/CorsairVengeanceLPX8GBDDR43000MHz.jpg'),
(3, 'G.Skill Ripjaws V 16GB DDR4 3600MHz', 1, 4, 54.99, 40, '/images/products/G.SkillRipjawsV16GBDDR43600MHz.jpeg'),
(4, 'ADATA XPG GAMMIX D30 8GB DDR4 2666MHz', 1, 3, 29.99, 29.99, '/images/products/ADATAXPGGAMMIXD308GBDDR42666MHz.jpeg'),
(5, 'Patriot Viper Steel 32GB DDR4 3200MHz', 1, 4, 84.99, 80, '/images/products/PatriotViperSteel32GBDDR43200MHz.jpeg'),
(6, 'Intel Core i5-12400F', 2, 1, 174.5, 174.5, '/images/products/IntelCorei5-12400F.jpeg'),
(7, 'AMD Ryzen 5 5600X', 2, 2, 279.99, 249.99, '/images/products/AMDRyzen55600X.jpeg'),
(8, 'Intel Core i7-12700K', 2, 1, 119.99, 119.99, '/images/products/IntelCorei7-12700K.jpeg'),
(9, 'AMD Ryzen 7 5800X', 2, 2, 49.99, 49.99, '/images/products/AMDRyzen75800X.jpeg'),
(10, 'Intel Core i3-12100', 2, 1, 34.99, 34.99, '/images/products/IntelCorei3-12100.jpeg'),
(11, 'Samsung 980 Pro 1TB NVMe', 3, 5, 54.99, 54.99, '/images/products/Samsung980Pro1TBNVMe.jpeg'),
(12, 'Kingston A2000 500GB NVMe', 3, 3, 29.99, 29.99, '/images/products/KingstonA2000500GBNVMe.jpg'),
(13, 'WD Blue SN570 1TB NVMe', 3, 3, 84.99, 80, '/images/products/WDBlueSN5701TBNVMe.png'),
(14, 'Crucial MX500 500GB SATA', 3, 3, 174.99, 150, '/images/products/CrucialMX500500GBSATA.jpeg'),
(15, 'ADATA SU800 1TB SATA', 3, 3, 159.99, 159.99, '/images/products/ADATASU8001TBSATA.jpeg'),
(16, 'NVIDIA RTX 3060 12GB', 4, 6, 349.99, 300, '/images/products/NVIDIARTX306012GB.jpeg'),
(17, 'MSI Radeon RX 6600 8GB', 4, 7, 279.99, 279.99, '/images/products/MSIRadeonRX66008GB.jpeg'),
(18, 'Gigabyte RTX 3070 8GB', 4, 6, 119.99, 119.99, '/images/products/GigabyteRTX30708GB.jpeg'),
(19, 'ASUS Radeon RX 6700 XT 12GB', 4, 7, 129.99, 129.99, '/images/products/ASUSRadeonRX6700XT12GB.jpeg'),
(20, 'ZOTAC GTX 1650 4GB', 4, 6, 49.99, 49.99, '/images/products/ZOTACGTX16504GB.jpeg');

-- --------------------------------------------------------

--
-- Table structure for table `ramdetails`
--

DROP TABLE IF EXISTS `ramdetails`;
CREATE TABLE `ramdetails` (
  `ProductID` int(11) NOT NULL,
  `CapacityGB` int(11) DEFAULT NULL,
  `SpeedMHz` int(11) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `ramdetails`
--

INSERT INTO `ramdetails` (`ProductID`, `CapacityGB`, `SpeedMHz`, `Type`) VALUES
(1, 16, 3200, 'DDR4'),
(2, 8, 3000, 'DDR4'),
(3, 16, 3600, 'DDR4'),
(4, 8, 2666, 'DDR4'),
(5, 32, 3200, 'DDR4');

-- --------------------------------------------------------

--
-- Table structure for table `ssddetails`
--

DROP TABLE IF EXISTS `ssddetails`;
CREATE TABLE `ssddetails` (
  `ProductID` int(11) NOT NULL,
  `CapacityGB` int(11) DEFAULT NULL,
  `Interface` varchar(50) DEFAULT NULL,
  `Type` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `ssddetails`
--

INSERT INTO `ssddetails` (`ProductID`, `CapacityGB`, `Interface`, `Type`) VALUES
(11, 1000, 'NVMe', 'M.2'),
(12, 500, 'NVMe', 'M.2'),
(13, 1000, 'NVMe', 'M.2'),
(14, 500, 'SATA', '2.5\"'),
(15, 1000, 'SATA', '2.5\"');

-- --------------------------------------------------------

--
-- Table structure for table `vgadetails`
--

DROP TABLE IF EXISTS `vgadetails`;
CREATE TABLE `vgadetails` (
  `ProductID` int(11) NOT NULL,
  `MemoryGB` int(11) DEFAULT NULL,
  `Chipset` varchar(100) DEFAULT NULL,
  `Interface` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `vgadetails`
--

INSERT INTO `vgadetails` (`ProductID`, `MemoryGB`, `Chipset`, `Interface`) VALUES
(16, 12, 'RTX 3060', 'PCIe 4.0'),
(17, 8, 'RX 6600', 'PCIe 4.0'),
(18, 8, 'RTX 3070', 'PCIe 4.0'),
(19, 12, 'RX 6700 XT', 'PCIe 4.0'),
(20, 4, 'GTX 1650', 'PCIe 3.0');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `brand`
--
ALTER TABLE `brand`
  ADD PRIMARY KEY (`BrandID`);

--
-- Indexes for table `cartitem`
--
ALTER TABLE `cartitem`
  ADD PRIMARY KEY (`CartItemId`),
  ADD KEY `CustomerId` (`CustomerId`),
  ADD KEY `ProductId` (`ProductId`);

--
-- Indexes for table `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`CategoryID`);

--
-- Indexes for table `cpudetails`
--
ALTER TABLE `cpudetails`
  ADD PRIMARY KEY (`ProductID`);

--
-- Indexes for table `customer`
--
ALTER TABLE `customer`
  ADD PRIMARY KEY (`CustomerId`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `fk_order` (`OrderID`);

--
-- Indexes for table `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`ProductID`);

--
-- Indexes for table `order`
--
ALTER TABLE `order`
  ADD PRIMARY KEY (`OrderID`),
  ADD KEY `CustomerID` (`CustomerID`);

--
-- Indexes for table `orderitem`
--
ALTER TABLE `orderitem`
  ADD PRIMARY KEY (`OrderItemID`),
  ADD KEY `OrderID` (`OrderID`),
  ADD KEY `ProductID` (`ProductID`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`ProductID`),
  ADD KEY `CategoryID` (`CategoryID`),
  ADD KEY `BrandID` (`BrandID`);

--
-- Indexes for table `ramdetails`
--
ALTER TABLE `ramdetails`
  ADD PRIMARY KEY (`ProductID`);

--
-- Indexes for table `ssddetails`
--
ALTER TABLE `ssddetails`
  ADD PRIMARY KEY (`ProductID`);

--
-- Indexes for table `vgadetails`
--
ALTER TABLE `vgadetails`
  ADD PRIMARY KEY (`ProductID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `brand`
--
ALTER TABLE `brand`
  MODIFY `BrandID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `cartitem`
--
ALTER TABLE `cartitem`
  MODIFY `CartItemId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `category`
--
ALTER TABLE `category`
  MODIFY `CategoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `customer`
--
ALTER TABLE `customer`
  MODIFY `CustomerId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `order`
--
ALTER TABLE `order`
  MODIFY `OrderID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `orderitem`
--
ALTER TABLE `orderitem`
  MODIFY `OrderItemID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `product`
--
ALTER TABLE `product`
  MODIFY `ProductID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `cartitem`
--
ALTER TABLE `cartitem`
  ADD CONSTRAINT `cartitem_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customer` (`CustomerId`),
  ADD CONSTRAINT `cartitem_ibfk_2` FOREIGN KEY (`ProductId`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `cpudetails`
--
ALTER TABLE `cpudetails`
  ADD CONSTRAINT `cpudetails_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `customer`
--
ALTER TABLE `customer`
  ADD CONSTRAINT `fk_order` FOREIGN KEY (`OrderID`) REFERENCES `order` (`OrderID`);

--
-- Constraints for table `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `order`
--
ALTER TABLE `order`
  ADD CONSTRAINT `order_ibfk_1` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`CustomerId`);

--
-- Constraints for table `orderitem`
--
ALTER TABLE `orderitem`
  ADD CONSTRAINT `orderitem_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `order` (`OrderID`),
  ADD CONSTRAINT `orderitem_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `product_ibfk_1` FOREIGN KEY (`CategoryID`) REFERENCES `category` (`CategoryID`),
  ADD CONSTRAINT `product_ibfk_2` FOREIGN KEY (`BrandID`) REFERENCES `brand` (`BrandID`);

--
-- Constraints for table `ramdetails`
--
ALTER TABLE `ramdetails`
  ADD CONSTRAINT `ramdetails_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `ssddetails`
--
ALTER TABLE `ssddetails`
  ADD CONSTRAINT `ssddetails_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Constraints for table `vgadetails`
--
ALTER TABLE `vgadetails`
  ADD CONSTRAINT `vgadetails_ibfk_1` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
