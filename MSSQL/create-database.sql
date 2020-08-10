CREATE DATABASE SessionDatabase;
GO
USE SessionDatabase;
GO
CREATE TABLE [dbo].[Session](
	[Id] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[UserAdress] [nvarchar](max) NULL,
	[IdVideo] [nvarchar](max) NULL,
	[RequestTime] [int] NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO