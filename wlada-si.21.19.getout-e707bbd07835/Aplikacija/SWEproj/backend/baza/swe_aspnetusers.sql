-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: swe
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(256) NOT NULL,
  `Ime` text NOT NULL,
  `Prezime` text NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` text,
  `SecurityStamp` text,
  `ConcurrencyStamp` text,
  `PhoneNumber` text,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` timestamp NULL DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`),
  KEY `Username_sto_indx` (`UserName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('227207ee-80ec-4cd4-be41-aa2fc44e6ef1','Vukasin','Popovic','vukasin','VUKASIN','vukasinp@elfak.rs','VUKASINP@ELFAK.RS',0,'AQAAAAEAACcQAAAAELjJz5bd45ZPI6C8hvESyjod6kuVMRJxQQtTCu5Yn17Jp7benwvtHTxPG7C9CnRddg==','DPMOJ52UQ4QVCPI7YM4DDWVNZ4NU75OZ','38b1b8a2-c7e7-44af-939d-ae69929a8c8f','0632131253',0,0,NULL,1,0),('428228b4-d200-4032-b883-43aa0b15e93f','Legija','Popovic','legija','LEGIJA','vukasisnp@elfak.rs','VUKASISNP@ELFAK.RS',0,'AQAAAAEAACcQAAAAEJDBIdfEtZs+luqj6SBTxVK0WhLAjxgX3ZcN4uKu8JA5k0Ds20itjktcLlbV8gPJZA==','3GJYNMPNJJQEFJQKLANBPA65THK4FAV2','74b6fc96-04fb-44f4-8d5b-a6791fd4ea46','0632131253',0,0,NULL,1,0),('67996f9b-6d1a-4f99-80dc-55659b2e19af','Filip','Nikolic','dadada','DADADA','nekinovi@yahoo.com','NEKINOVI@YAHOO.COM',0,'AQAAAAEAACcQAAAAEGOG/Tz0cMorA5N62Q4TbVxsr69wObDsjh0A7vt0kGlHyXsEt+spU3PBD7zL2+iDCw==','75UMIHW53537PK4B7JAFZRQY4PW4JEB6','bb853a29-6b3b-40a8-9569-511080fef59b','0512402142',0,0,NULL,1,0),('ec979f7b-5d9a-4d5c-8ce7-0863f5d31aab','Mateja','Pancic','matetebra','MATETEBRA','mateee@elfak.rs','MATEEE@ELFAK.RS',0,'AQAAAAEAACcQAAAAENHIP+jzOJPhKhKp0tREcx0Snt8epdZgZ3L6vj+Jp0jiEdfvzAs3WHAhIE+PS46yCA==','IG6YTM3XTCCSFVWDGNO4L74WP3D5YG4J','a8fc5155-6a5f-49b1-b047-e45e3ae882d4','0632131253',0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-08 18:54:33
