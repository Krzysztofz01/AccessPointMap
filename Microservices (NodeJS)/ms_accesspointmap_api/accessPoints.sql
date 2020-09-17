CREATE TABLE `accesspoints` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `bssid` VARCHAR(25) NOT NULL,
  `ssid` VARCHAR(40) NOT NULL,
  `freq` INT(5) NOT NULL,
  `signalLevel` INT(5) NOT NULL,
  `longitude` DOUBLE NOT NULL,
  `latitude` DOUBLE NOT NULL,
  `lowSignalLevel` INT(5) NOT NUll,
  `lowLongitude` DOUBLE NOT NULL,
  `lowLatitude` DOUBLE NOT NULL,
  `signalArea` DOUBLE NOT NULL,
  `security` VARCHAR(65) NOT NULL,
  `vendor` VARCHAR(60) NOT NULL,
  `added` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARACTER SET = utf8;