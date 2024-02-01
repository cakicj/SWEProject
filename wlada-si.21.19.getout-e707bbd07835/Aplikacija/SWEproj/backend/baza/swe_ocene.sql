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
-- Table structure for table `ocene`
--

DROP TABLE IF EXISTS `ocene`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ocene` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ocena` int(11) NOT NULL,
  `kafic_id` int(11) NOT NULL,
  `korisnik_id` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `kafic_id_FK2_idx` (`kafic_id`),
  KEY `korisnik_id_FK2_idx` (`korisnik_id`),
  CONSTRAINT `kafic_id_FK2` FOREIGN KEY (`kafic_id`) REFERENCES `kafic` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `korisnik_id_FK2` FOREIGN KEY (`korisnik_id`) REFERENCES `aspnetusers` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ocene`
--

LOCK TABLES `ocene` WRITE;
/*!40000 ALTER TABLE `ocene` DISABLE KEYS */;
INSERT INTO `ocene` VALUES (1,5,5,'67996f9b-6d1a-4f99-80dc-55659b2e19af'),(2,3,4,'67996f9b-6d1a-4f99-80dc-55659b2e19af'),(3,4,4,'ec979f7b-5d9a-4d5c-8ce7-0863f5d31aab'),(4,1,5,'ec979f7b-5d9a-4d5c-8ce7-0863f5d31aab'),(6,4,1,'67996f9b-6d1a-4f99-80dc-55659b2e19af'),(7,4,2,'67996f9b-6d1a-4f99-80dc-55659b2e19af');
/*!40000 ALTER TABLE `ocene` ENABLE KEYS */;
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
