--THIS IS USING MYSQL DATABASE
CREATE DATABASE redtech_db;
use redtech_db;

CREATE TABLE `order_types` (
  `TypeID` int NULL ,
  `TypeName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`TypeID`)
);

INSERT INTO YourTableName (TypeID, TypeName)
VALUES
    (1, 'Standard'),
    (2, 'SaleOrder'),
    (3, 'PurchaseOrder'),
    (4, 'TransferOrder'),
    (5, 'ReturnOrder');
    
    CREATE TABLE `orders` (
  `OrderID` int NOT NULL AUTO_INCREMENT,
  `TypeID` int DEFAULT NULL,
  `CustomerName` varchar(255) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `CreatedByUsername` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`OrderID`),
  KEY `TypeID` (`TypeID`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`TypeID`) REFERENCES `order_types` (`TypeID`)
);