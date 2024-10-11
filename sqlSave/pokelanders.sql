-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : ven. 11 oct. 2024 à 12:06
-- Version du serveur : 8.3.0
-- Version de PHP : 8.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `pokelanders`
--

-- --------------------------------------------------------

--
-- Structure de la table `elementary_types`
--

DROP TABLE IF EXISTS `elementary_types`;
CREATE TABLE IF NOT EXISTS `elementary_types` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` tinytext NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `elementary_types`
--

INSERT INTO `elementary_types` (`ID`, `Name`) VALUES
(1, 'Water'),
(2, 'Fire'),
(3, 'Grass'),
(4, 'Light'),
(5, 'Dark');

-- --------------------------------------------------------

--
-- Structure de la table `has_type`
--

DROP TABLE IF EXISTS `has_type`;
CREATE TABLE IF NOT EXISTS `has_type` (
  `ID_lander` int NOT NULL,
  `ID_type` int NOT NULL,
  KEY `FK_ID_Lander_1` (`ID_lander`),
  KEY `FK_ID_Type_1` (`ID_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `has_type`
--

INSERT INTO `has_type` (`ID_lander`, `ID_type`) VALUES
(1, 1),
(2, 1),
(2, 4),
(3, 1),
(3, 4);

-- --------------------------------------------------------

--
-- Structure de la table `landers`
--

DROP TABLE IF EXISTS `landers`;
CREATE TABLE IF NOT EXISTS `landers` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` tinytext NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `landers`
--

INSERT INTO `landers` (`ID`, `Name`, `Description`) VALUES
(1, 'Aquapix', 'Aquapix est un Pokémon farceur qui vit au bord des rivières et des lacs. Grâce à sa queue en forme de fontaine, il peut projeter de fines gouttelettes scintillantes, capables de calmer les esprits agités. Les marins racontent que les arcs-en-ciel apparaissent souvent après le passage d\'un Aquapix.'),
(2, 'Luminorine', 'Luminorine est vénérée dans certaines cultures côtières comme le gardien des océans. Grâce à sa danse aquatique, elle est capable de calmer même les tempêtes les plus furieuses. Ses chants, portés par le vent marin, inspirent espoir et courage à ceux qui les entendent.'),
(3, 'Ocealythe', 'Ocealythe est vénéré comme un protecteur divin des océans et des créatures marines. Ses pouvoirs mystiques sont capables d\'influencer les courants océaniques et même de contrôler le climat. Les légendes racontent que lorsque Ocealythe apparaît, les tempêtes s\'apaisent et les eaux deviennent claires et lumineuses.');

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `has_type`
--
ALTER TABLE `has_type`
  ADD CONSTRAINT `FK_ID_Lander_1` FOREIGN KEY (`ID_lander`) REFERENCES `landers` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_ID_Type_1` FOREIGN KEY (`ID_type`) REFERENCES `elementary_types` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
