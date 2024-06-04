-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 04, 2024 at 10:25 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `save_it_finances`
--

-- --------------------------------------------------------

--
-- Table structure for table `convertor_quotes`
--

CREATE TABLE `convertor_quotes` (
  `quote_id` int(10) UNSIGNED NOT NULL,
  `customer_id` int(10) UNSIGNED NOT NULL,
  `transaction_type` enum('buy','sell','cross-currency') NOT NULL,
  `currency_from_id` int(10) UNSIGNED NOT NULL,
  `currency_to_id` int(10) UNSIGNED NOT NULL,
  `currency_amount` decimal(10,2) NOT NULL CHECK (`currency_amount` > 0),
  `exchange_rate` decimal(10,4) NOT NULL CHECK (`exchange_rate` > 0),
  `fee` decimal(10,2) NOT NULL CHECK (`fee` >= 0),
  `quote_total` decimal(10,2) NOT NULL CHECK (`quote_total` >= 0),
  `quote_time` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `currency_rates`
--

CREATE TABLE `currency_rates` (
  `currency_rate_id` int(10) UNSIGNED NOT NULL,
  `currency_name` varchar(3) NOT NULL CHECK (`currency_name` regexp '^[A-Z]{3}$'),
  `value` decimal(10,4) NOT NULL CHECK (`value` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `customer_id` int(10) UNSIGNED NOT NULL,
  `full_name` varchar(255) NOT NULL CHECK (`full_name` regexp '^[A-Za-z\\s\'-]+$'),
  `email` varchar(255) NOT NULL CHECK (`email` regexp '^[^@]+@[^@]+\\.[^@]{2,}$'),
  `phone` varchar(11) NOT NULL CHECK (`phone` regexp '^[0-9]{10,11}$'),
  `address` varchar(255) NOT NULL CHECK (`address` regexp '^[0-9A-Za-z]+[0-9A-Za-z\\s,-]*$'),
  `postcode` varchar(10) NOT NULL CHECK (`postcode` regexp '^[0-9A-Za-z]+$')
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `error_logs`
--

CREATE TABLE `error_logs` (
  `log_id` int(10) UNSIGNED NOT NULL,
  `customer_id` int(10) UNSIGNED DEFAULT NULL,
  `error_type` varchar(255) NOT NULL,
  `error_message` text NOT NULL,
  `diagnostic_data` text DEFAULT NULL,
  `timestamp` datetime NOT NULL DEFAULT current_timestamp(),
  `user_id` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `estimated_tax`
--

CREATE TABLE `estimated_tax` (
  `tax_id` int(10) UNSIGNED NOT NULL,
  `product_id` int(10) UNSIGNED NOT NULL,
  `threshold_amount` decimal(10,2) DEFAULT NULL CHECK (`threshold_amount` >= 0 or `threshold_amount` is null),
  `tax_rate` decimal(5,2) NOT NULL CHECK (`tax_rate` >= 0 and `tax_rate` <= 100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `fee_tiers`
--

CREATE TABLE `fee_tiers` (
  `fee_tier_id` int(10) UNSIGNED NOT NULL,
  `limit_id` int(10) UNSIGNED NOT NULL,
  `min_amount` decimal(10,2) NOT NULL CHECK (`min_amount` >= 0),
  `max_amount` decimal(10,2) DEFAULT NULL CHECK (`max_amount` >= `min_amount` or `max_amount` is null),
  `fee_percentage` decimal(5,2) NOT NULL CHECK (`fee_percentage` >= 0 and `fee_percentage` <= 100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `investment_products`
--

CREATE TABLE `investment_products` (
  `product_id` int(10) UNSIGNED NOT NULL,
  `product_name` enum('Basic Saving Plan','Saving Plan Plus','Managed Stock Investments') NOT NULL,
  `max_annual_investment` decimal(10,2) DEFAULT NULL,
  `min_monthly_investment` decimal(10,2) NOT NULL CHECK (`min_monthly_investment` > 0),
  `min_initial_lumpsum` decimal(10,2) DEFAULT NULL CHECK (`min_initial_lumpsum` >= 0 or `min_initial_lumpsum` is null),
  `predicted_return_low` decimal(5,2) NOT NULL CHECK (`predicted_return_low` >= 0 and `predicted_return_low` <= 100),
  `predicted_return_high` decimal(5,2) NOT NULL CHECK (`predicted_return_high` >= `predicted_return_low` and `predicted_return_high` <= 100),
  `monthly_fees_percentage` decimal(5,4) NOT NULL CHECK (`monthly_fees_percentage` >= 0 and `monthly_fees_percentage` <= 100),
  `last_update` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `investment_quotes`
--

CREATE TABLE `investment_quotes` (
  `quote_id` int(10) UNSIGNED NOT NULL,
  `customer_id` int(10) UNSIGNED NOT NULL,
  `product_id` int(10) UNSIGNED NOT NULL,
  `initial_lump_sum` decimal(10,2) DEFAULT NULL,
  `monthly_investment` decimal(10,2) NOT NULL CHECK (`monthly_investment` >= 0),
  `returns_1_year_low` decimal(10,2) NOT NULL CHECK (`returns_1_year_low` >= 0),
  `returns_1_year_high` decimal(10,2) NOT NULL CHECK (`returns_1_year_high` >= `returns_1_year_low`),
  `returns_5_years_low` decimal(10,2) NOT NULL CHECK (`returns_5_years_low` >= 0),
  `returns_5_years_high` decimal(10,2) NOT NULL CHECK (`returns_5_years_high` >= `returns_5_years_low`),
  `returns_10_years_low` decimal(10,2) NOT NULL CHECK (`returns_10_years_low` >= 0),
  `profit_1_year_low` decimal(10,2) NOT NULL,
  `profit_1_year_high` decimal(10,2) NOT NULL,
  `profit_5_years_low` decimal(10,2) NOT NULL,
  `profit_5_years_high` decimal(10,2) NOT NULL,
  `profit_10_years_low` decimal(10,2) NOT NULL,
  `profit_10_years_high` decimal(10,2) NOT NULL,
  `returns_10_years_high` decimal(10,2) NOT NULL,
  `fees_1_year_low` decimal(10,2) NOT NULL,
  `fees_1_year_high` decimal(10,2) NOT NULL,
  `fees_5_years_low` decimal(10,2) NOT NULL,
  `fees_5_years_high` decimal(10,2) NOT NULL,
  `fees_10_years_low` decimal(10,2) NOT NULL,
  `fees_10_years_high` decimal(10,2) NOT NULL,
  `taxes_1_year_low` decimal(10,2) NOT NULL,
  `taxes_1_year_high` decimal(10,2) NOT NULL,
  `taxes_5_years_low` decimal(10,2) NOT NULL,
  `taxes_5_years_high` decimal(10,2) NOT NULL,
  `taxes_10_years_low` decimal(10,2) NOT NULL,
  `taxes_10_years_high` decimal(10,2) NOT NULL,
  `total_invested_1` decimal(10,2) NOT NULL,
  `total_invested_5` decimal(10,2) NOT NULL,
  `total_invested_10` decimal(10,2) NOT NULL,
  `timestamp` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `transaction_limits`
--

CREATE TABLE `transaction_limits` (
  `limit_id` int(10) UNSIGNED NOT NULL,
  `transaction_type` varchar(255) NOT NULL,
  `min_transaction_value` decimal(10,2) NOT NULL CHECK (`min_transaction_value` >= 0),
  `max_transaction_value` decimal(10,2) NOT NULL CHECK (`max_transaction_value` >= `min_transaction_value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `user_id` int(10) UNSIGNED NOT NULL,
  `full_name` varchar(255) NOT NULL CHECK (`full_name` regexp '^[A-Za-z\\s\'-]+$'),
  `email` varchar(255) NOT NULL CHECK (`email` regexp '^[^@]+@[^@]+\\.[^@]{2,}$'),
  `phone` varchar(11) NOT NULL CHECK (`phone` regexp '^[0-9]{10,11}$'),
  `address` varchar(255) NOT NULL CHECK (`address` regexp '^[0-9A-Za-z]+[0-9A-Za-z\\s,-]*$'),
  `postcode` varchar(10) NOT NULL CHECK (`postcode` regexp '^[0-9A-Za-z]+$'),
  `username` varchar(255) NOT NULL CHECK (`username` regexp '^[A-Za-z0-9._-]{3,255}$'),
  `password_hash` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `convertor_quotes`
--
ALTER TABLE `convertor_quotes`
  ADD PRIMARY KEY (`quote_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `currency_from_id` (`currency_from_id`),
  ADD KEY `currency_to_id` (`currency_to_id`);

--
-- Indexes for table `currency_rates`
--
ALTER TABLE `currency_rates`
  ADD PRIMARY KEY (`currency_rate_id`),
  ADD UNIQUE KEY `currency_name` (`currency_name`);

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`customer_id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indexes for table `error_logs`
--
ALTER TABLE `error_logs`
  ADD PRIMARY KEY (`log_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `error_logs_ibfk_2` (`user_id`);

--
-- Indexes for table `estimated_tax`
--
ALTER TABLE `estimated_tax`
  ADD PRIMARY KEY (`tax_id`),
  ADD KEY `product_id` (`product_id`);

--
-- Indexes for table `fee_tiers`
--
ALTER TABLE `fee_tiers`
  ADD PRIMARY KEY (`fee_tier_id`),
  ADD KEY `limit_id` (`limit_id`);

--
-- Indexes for table `investment_products`
--
ALTER TABLE `investment_products`
  ADD PRIMARY KEY (`product_id`);

--
-- Indexes for table `investment_quotes`
--
ALTER TABLE `investment_quotes`
  ADD PRIMARY KEY (`quote_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `product_id` (`product_id`);

--
-- Indexes for table `transaction_limits`
--
ALTER TABLE `transaction_limits`
  ADD PRIMARY KEY (`limit_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `convertor_quotes`
--
ALTER TABLE `convertor_quotes`
  MODIFY `quote_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `currency_rates`
--
ALTER TABLE `currency_rates`
  MODIFY `currency_rate_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `customer_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `error_logs`
--
ALTER TABLE `error_logs`
  MODIFY `log_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `estimated_tax`
--
ALTER TABLE `estimated_tax`
  MODIFY `tax_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fee_tiers`
--
ALTER TABLE `fee_tiers`
  MODIFY `fee_tier_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `investment_products`
--
ALTER TABLE `investment_products`
  MODIFY `product_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `investment_quotes`
--
ALTER TABLE `investment_quotes`
  MODIFY `quote_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `transaction_limits`
--
ALTER TABLE `transaction_limits`
  MODIFY `limit_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `user_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `convertor_quotes`
--
ALTER TABLE `convertor_quotes`
  ADD CONSTRAINT `convertor_quotes_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  ADD CONSTRAINT `convertor_quotes_ibfk_2` FOREIGN KEY (`currency_from_id`) REFERENCES `currency_rates` (`currency_rate_id`),
  ADD CONSTRAINT `convertor_quotes_ibfk_3` FOREIGN KEY (`currency_to_id`) REFERENCES `currency_rates` (`currency_rate_id`);

--
-- Constraints for table `error_logs`
--
ALTER TABLE `error_logs`
  ADD CONSTRAINT `error_logs_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  ADD CONSTRAINT `error_logs_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`);

--
-- Constraints for table `estimated_tax`
--
ALTER TABLE `estimated_tax`
  ADD CONSTRAINT `estimated_tax_ibfk_1` FOREIGN KEY (`product_id`) REFERENCES `investment_products` (`product_id`);

--
-- Constraints for table `fee_tiers`
--
ALTER TABLE `fee_tiers`
  ADD CONSTRAINT `fee_tiers_ibfk_1` FOREIGN KEY (`limit_id`) REFERENCES `transaction_limits` (`limit_id`);

--
-- Constraints for table `investment_quotes`
--
ALTER TABLE `investment_quotes`
  ADD CONSTRAINT `investment_quotes_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  ADD CONSTRAINT `investment_quotes_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `investment_products` (`product_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
