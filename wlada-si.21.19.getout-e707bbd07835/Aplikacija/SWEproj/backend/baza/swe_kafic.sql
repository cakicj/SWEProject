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
-- Table structure for table `kafic`
--

DROP TABLE IF EXISTS `kafic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `kafic` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `naziv` varchar(45) NOT NULL,
  `ocena` float DEFAULT NULL,
  `adresa` varchar(100) NOT NULL,
  `telefon` varchar(45) NOT NULL,
  `dogadjaj` varchar(100) DEFAULT NULL,
  `ukupan_kapacitet` int(11) DEFAULT NULL,
  `radno_vreme` varchar(50) DEFAULT NULL,
  `mapaX` float DEFAULT NULL,
  `mapaY` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kafic`
--

LOCK TABLES `kafic` WRITE;
/*!40000 ALTER TABLE `kafic` DISABLE KEYS */;
INSERT INTO `kafic` VALUES (1,'Pleasure',4,'Kopitareva 7','+3816395310','Acustic night- Berlin duo',22,NULL,3.13213,5.32312),(2,'Pleasure Center',4,'Drvarska 2','+3816173452','Band Blackberies',28,NULL,0,0),(3,'Pleasure Cair',0,'9. Brigade','+38162335842','nema',34,NULL,0,0),(4,'Pleasure Ambasador',3.5,'Vozda Karadjordja 3','+3816073421','nema',24,NULL,0,0),(5,'Novi',3,'Bulevar Nemanjica','06353423','',10,NULL,0,0);
/*!40000 ALTER TABLE `kafic` ENABLE KEYS */;
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
