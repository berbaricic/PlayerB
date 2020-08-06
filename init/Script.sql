IF NOT EXISTS(SELECT * FROM sys.databases WHERE name='SessionDatabase')
    CREATE DATABASE [SessionDatabase]
GO
USE [dba]