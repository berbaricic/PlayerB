CREATE DATABASE SessionDatabase;
GO
USE SessionDatabase;
GO
CREATE TABLE [Session](
	[Id] [varchar](max) NOT NULL,
	[Status] [varchar](max) NULL,
	[UserAdress] [varchar](max) NULL,
	[IdVideo] [varchar](max) NULL,
	[RequestTime] [int] NULL
)
GO