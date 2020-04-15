/*
 Navicat Premium Data Transfer

 Source Server         : a
 Source Server Type    : MySQL
 Source Server Version : 50720
 Source Host           : localhost:3306
 Source Schema         : test

 Target Server Type    : MySQL
 Target Server Version : 50720
 File Encoding         : 65001

 Date: 21/03/2020 04:58:45
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for MASTER
-- ----------------------------
DROP TABLE IF EXISTS `MASTER`;
CREATE TABLE `MASTER` (
  `wordId` int(11) NOT NULL,
  `spell` longtext  NOT NULL,
  `phoneticSymbol` longtext  NOT NULL,
  `explaination` longtext  NOT NULL,
  `sentenceEN` longtext  NOT NULL,
  `sentenceCH` longtext  NOT NULL,
  `pronouncationURL` longtext  NOT NULL,
  `wordLength` int(11) NOT NULL,
  `learnedTimes` int(11) NOT NULL,
  `ungraspTimes` int(11) NOT NULL,
  `isFamiliar` int(11) NOT NULL,
  `backupPronounciationURL` longtext  NOT NULL,
  PRIMARY KEY (`wordId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for MEDIUM
-- ----------------------------
DROP TABLE IF EXISTS `MEDIUM`;
CREATE TABLE `MEDIUM` (
  `wordId` int(11) NOT NULL,
  `spell` longtext  NOT NULL,
  `phoneticSymbol` longtext  NOT NULL,
  `explaination` longtext  NOT NULL,
  `sentenceEN` longtext  NOT NULL,
  `sentenceCH` longtext  NOT NULL,
  `pronouncationURL` longtext  NOT NULL,
  `wordLength` int(11) NOT NULL,
  `learnedTimes` int(11) NOT NULL,
  `ungraspTimes` int(11) NOT NULL,
  `isFamiliar` int(11) NOT NULL,
  `backupPronounciationURL` longtext  NOT NULL,
  PRIMARY KEY (`wordId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for SIMPLE
-- ----------------------------
DROP TABLE IF EXISTS `SIMPLE`;
CREATE TABLE `SIMPLE` (
  `wordId` int(11) NOT NULL,
  `spell` longtext  NOT NULL,
  `phoneticSymbol` longtext  NOT NULL,
  `explaination` longtext  NOT NULL,
  `sentenceEN` longtext  NOT NULL,
  `sentenceCH` longtext  NOT NULL,
  `pronouncationURL` longtext  NOT NULL,
  `wordLength` int(11) NOT NULL,
  `learnedTimes` int(11) NOT NULL,
  `ungraspTimes` int(11) NOT NULL,
  `isFamiliar` int(11) NOT NULL,
  `backupPronounciationURL` longtext  NOT NULL,
  PRIMARY KEY (`wordId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `userId` int(11) NOT NULL AUTO_INCREMENT COMMENT '玩家 ID',
  `name` varchar(30)  DEFAULT NULL COMMENT '玩家名',
  `pwd` varchar(255)  DEFAULT NULL COMMENT '密码',
  `score` varchar(255)  DEFAULT NULL COMMENT '分数',
  `date` datetime timestamp DEFAULT CURRENT_TIMESTAMP COMMENT '注册时间',
  `email` varchar(255)  DEFAULT NULL COMMENT '个人邮箱',
  `phone` varchar(255)  DEFAULT NULL COMMENT '联系电话',
  PRIMARY KEY (`userId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user_friend
-- ----------------------------
DROP TABLE IF EXISTS `user_friend`;
CREATE TABLE `user_friend` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '好友表 ID',
  `userId` int(11) NOT NULL COMMENT '玩家 ID',
  `userId2` int(11) NOT NULL COMMENT '玩家好友 ID',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `user_friend_fk_1` (`userId`),
  KEY `user_friend_fk_2` (`userId2`),
  CONSTRAINT `user_friend_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`userId`),
  CONSTRAINT `user_friend_ibfk_2` FOREIGN KEY (`userId2`) REFERENCES `user` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user_vocab
-- ----------------------------
DROP TABLE IF EXISTS `user_vocab`;
CREATE TABLE `user_vocab` (
  `vocabId` int(11) NOT NULL AUTO_INCREMENT COMMENT '单词表 ID',
  `vocabName` varchar(30)  NOT NULL COMMENT '单词表名字',
  PRIMARY KEY (`vocabId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for user_vocab_relation
-- ----------------------------
DROP TABLE IF EXISTS `user_vocab_relation`;
CREATE TABLE `user_vocab_relation` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL COMMENT '玩家 ID',
  `vocabId` int(11) NOT NULL COMMENT '玩家好友 ID',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `user_vocab_fk_1` (`userId`),
  KEY `user_vocab_fk_2` (`vocabId`),
  CONSTRAINT `user_vocab_relation_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`userId`),
  CONSTRAINT `user_vocab_relation_ibfk_2` FOREIGN KEY (`vocabId`) REFERENCES `user_vocab` (`vocabId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Table structure for weapon
-- ----------------------------
DROP TABLE IF EXISTS `weapon`;
CREATE TABLE `weapon` (
  `weaponId` int(11) NOT NULL AUTO_INCREMENT COMMENT '武器 ID',
  `name` varchar(255)  DEFAULT NULL COMMENT '武器名',
  `damage` int(11) DEFAULT NULL COMMENT '伤害值',
  `aimSpeed` varchar(255)  DEFAULT NULL COMMENT '瞄准速度',
  `clipSize` int(11) DEFAULT NULL COMMENT '子弹数',
  `gunShot` varchar(255)  DEFAULT NULL COMMENT '射击声',
  `shotPitch` varchar(255)  DEFAULT NULL COMMENT '射击声音力度',
  `prefab` varchar(255)  DEFAULT NULL COMMENT '武器预制体',
  `reloadTime` float DEFAULT NULL COMMENT '装载时间',
  PRIMARY KEY (`weaponId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

SET FOREIGN_KEY_CHECKS = 1;
