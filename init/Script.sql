CREATE DATABASE SessionDatabase;
USE SessionDatabase;

CREATE TABLE Session (
	Id varchar(50) PRIMARY KEY NOT NULL,
	Status varchar(50) NOT NULL,
	UserAdress varchar(50) NULL,
	IdVideo varchar(50) NOT NULL,
	RequestTime int NOT NULL
);
