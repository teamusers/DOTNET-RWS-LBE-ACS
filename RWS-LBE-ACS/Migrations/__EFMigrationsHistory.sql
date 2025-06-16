USE [LBE-ACS]
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
 [MigrationId] [nvarchar](150) NOT NULL,
 [ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
 [MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


INSERT INTO [dbo].[__EFMigrationsHistory]
           ([MigrationId]
           ,[ProductVersion])
     VALUES
           ('20250616025519_AddMissingChanges',
           '9.0.6') 