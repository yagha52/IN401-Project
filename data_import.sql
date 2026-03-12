-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 13, 2025 at 09:14 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";
/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */
;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */
;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */
;
/*!40101 SET NAMES utf8mb4 */
;
--
-- Database: `hardwarestore`
--

--
-- Dumping data for table `brand`
--

INSERT INTO `brand` (`BrandID`, `BrandName`)
VALUES (1, 'Intel'),
    (2, 'AMD'),
    (3, 'Kingston'),
    (4, 'Corsair'),
    (5, 'Samsung'),
    (6, 'NVIDIA'),
    (7, 'MSI');
--
-- Dumping data for table `category`
--

INSERT INTO `category` (`CategoryID`, `CategoryName`)
VALUES (1, 'RAM'),
    (2, 'CPU'),
    (3, 'SSD'),
    (4, 'VGA');
--
-- Dumping data for table `cpudetails`
--

INSERT INTO `cpudetails` (
        `ProductID`,
        `Cores`,
        `Threads`,
        `BaseClockGHz`,
        `BoostClockGHz`,
        `Socket`
    )
VALUES (6, 6, 12, 2.50, 4.40, 'LGA1700'),
    (7, 6, 12, 3.70, 4.60, 'AM4'),
    (8, 12, 20, 3.60, 5.00, 'LGA1700'),
    (9, 8, 16, 3.80, 4.70, 'AM4'),
    (10, 4, 8, 3.30, 4.30, 'LGA1700');
--
-- Dumping data for table `customer`
--

INSERT INTO `customer` (
        `CustomerId`,
        `FirstName`,
        `LastName`,
        `Email`,
        `Phone`,
        `PasswordHash`,
        `StreetAddress`,
        `City`,
        `State`,
        `ZipCode`,
        `Country`,
        `OrderId`
    )
VALUES (
        1,
        'Emma',
        'Williams',
        'emma.williams@example.com',
        '(212) 555-1234',
        'hashed_password_1',
        '742 Evergreen Terrace',
        'Springfield',
        'IL',
        NULL,
        NULL,
        NULL
    ),
    (
        2,
        'Liam',
        'Johnson',
        'liam.johnson@example.com',
        '(310) 555-5678',
        'hashed_password_2',
        '1600 Pennsylvania Ave',
        'Washington',
        'DC',
        NULL,
        NULL,
        NULL
    ),
    (
        3,
        'Olivia',
        'Brown',
        'olivia.brown@example.com',
        '(415) 555-9012',
        'hashed_password_3',
        '1 Infinite Loop',
        'Cupertino',
        'CA',
        NULL,
        NULL,
        NULL
    ),
    (
        4,
        'Maya',
        'Arrouk',
        'mayaarrouk@gmail.com',
        '76788056',
        'X0eshOFf6/f97LcXQRTVIi610d9pHc5yw1f+OdcujRI=',
        'mezher',
        'antelias',
        'maten',
        '00000',
        'lebanon',
        NULL
    ),
    (
        5,
        'Yara',
        'Eslim',
        'eslimyara@gmail.com',
        NULL,
        'XpdV9ZaPt5xrP2WcKkUEmKYlhcoFZWrpbpPZky17ntU=',
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL
    ),
    (
        7,
        'reine',
        'nahas',
        'reine.nahas@gmail.com',
        '76737123',
        'S0hx1UDVlXVQBbDLvY1+y8tH4t2iJaFmZ9bXST+h5Vc=',
        'zalka',
        'zalka',
        'maten',
        '00000',
        'LEBANON',
        NULL
    );
--
-- Dumping data for table `product`
--

INSERT INTO `product` (
        `ProductID`,
        `ProductName`,
        `CategoryID`,
        `BrandID`,
        `OriginalPrice`,
        `Price`,
        `image_url`
    )
VALUES (
        1,
        'Kingston HyperX Fury 16GB DDR4 3200MHz',
        1,
        3,
        49.99,
        49.99,
        '/images/products/KingstonHyperXFury16GBDDR43200MHz.jpg'
    ),
    (
        2,
        'Corsair Vengeance LPX 8GB DDR4 3000MHz',
        1,
        4,
        39.5,
        39.5,
        '/images/products/CorsairVengeanceLPX8GBDDR43000MHz.jpg'
    ),
    (
        3,
        'G.Skill Ripjaws V 16GB DDR4 3600MHz',
        1,
        4,
        54.99,
        40,
        '/images/products/G.SkillRipjawsV16GBDDR43600MHz.jpeg'
    ),
    (
        4,
        'ADATA XPG GAMMIX D30 8GB DDR4 2666MHz',
        1,
        3,
        29.99,
        29.99,
        '/images/products/ADATAXPGGAMMIXD308GBDDR42666MHz.jpeg'
    ),
    (
        5,
        'Patriot Viper Steel 32GB DDR4 3200MHz',
        1,
        4,
        84.99,
        80,
        '/images/products/PatriotViperSteel32GBDDR43200MHz.jpeg'
    ),
    (
        6,
        'Intel Core i5-12400F',
        2,
        1,
        174.5,
        174.5,
        '/images/products/IntelCorei5-12400F.jpeg'
    ),
    (
        7,
        'AMD Ryzen 5 5600X',
        2,
        2,
        279.99,
        249.99,
        '/images/products/AMDRyzen55600X.jpeg'
    ),
    (
        8,
        'Intel Core i7-12700K',
        2,
        1,
        119.99,
        119.99,
        '/images/products/IntelCorei7-12700K.jpeg'
    ),
    (
        9,
        'AMD Ryzen 7 5800X',
        2,
        2,
        49.99,
        49.99,
        '/images/products/AMDRyzen75800X.jpeg'
    ),
    (
        10,
        'Intel Core i3-12100',
        2,
        1,
        34.99,
        34.99,
        '/images/products/IntelCorei3-12100.jpeg'
    ),
    (
        11,
        'Samsung 980 Pro 1TB NVMe',
        3,
        5,
        54.99,
        54.99,
        '/images/products/Samsung980Pro1TBNVMe.jpeg'
    ),
    (
        12,
        'Kingston A2000 500GB NVMe',
        3,
        3,
        29.99,
        29.99,
        '/images/products/KingstonA2000500GBNVMe.jpg'
    ),
    (
        13,
        'WD Blue SN570 1TB NVMe',
        3,
        3,
        84.99,
        80,
        '/images/products/WDBlueSN5701TBNVMe.png'
    ),
    (
        14,
        'Crucial MX500 500GB SATA',
        3,
        3,
        174.99,
        150,
        '/images/products/CrucialMX500500GBSATA.jpeg'
    ),
    (
        15,
        'ADATA SU800 1TB SATA',
        3,
        3,
        159.99,
        159.99,
        '/images/products/ADATASU8001TBSATA.jpeg'
    ),
    (
        16,
        'NVIDIA RTX 3060 12GB',
        4,
        6,
        349.99,
        300,
        '/images/products/NVIDIARTX306012GB.jpeg'
    ),
    (
        17,
        'MSI Radeon RX 6600 8GB',
        4,
        7,
        279.99,
        279.99,
        '/images/products/MSIRadeonRX66008GB.jpeg'
    ),
    (
        18,
        'Gigabyte RTX 3070 8GB',
        4,
        6,
        119.99,
        119.99,
        '/images/products/GigabyteRTX30708GB.jpeg'
    ),
    (
        19,
        'ASUS Radeon RX 6700 XT 12GB',
        4,
        7,
        129.99,
        129.99,
        '/images/products/ASUSRadeonRX6700XT12GB.jpeg'
    ),
    (
        20,
        'ZOTAC GTX 1650 4GB',
        4,
        6,
        49.99,
        49.99,
        '/images/products/ZOTACGTX16504GB.jpeg'
    );
--
-- Dumping data for table `ramdetails`
--

INSERT INTO `ramdetails` (`ProductID`, `CapacityGB`, `SpeedMHz`, `Type`)
VALUES (1, 16, 3200, 'DDR4'),
    (2, 8, 3000, 'DDR4'),
    (3, 16, 3600, 'DDR4'),
    (4, 8, 2666, 'DDR4'),
    (5, 32, 3200, 'DDR4');
--
-- Dumping data for table `ssddetails`
--

INSERT INTO `ssddetails` (`ProductID`, `CapacityGB`, `Interface`, `Type`)
VALUES (11, 1000, 'NVMe', 'M.2'),
    (12, 500, 'NVMe', 'M.2'),
    (13, 1000, 'NVMe', 'M.2'),
    (14, 500, 'SATA', '2.5\"'),
    (15, 1000, 'SATA', '2.5\"');
--
-- Dumping data for table `vgadetails`
--

INSERT INTO `vgadetails` (`ProductID`, `MemoryGB`, `Chipset`, `Interface`)
VALUES (16, 12, 'RTX 3060', 'PCIe 4.0'),
    (17, 8, 'RX 6600', 'PCIe 4.0'),
    (18, 8, 'RTX 3070', 'PCIe 4.0'),
    (19, 12, 'RX 6700 XT', 'PCIe 4.0'),
    (20, 4, 'GTX 1650', 'PCIe 3.0');
COMMIT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */
;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */
;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */
;