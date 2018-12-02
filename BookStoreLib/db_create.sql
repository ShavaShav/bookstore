/********************************************************************************************************
 * DROP TABLES
 * Ref: https://stackoverflow.com/a/14997851
 * Tried a lot of simpler methods, but this is the only one that worked
 ********************************************************************************************************
*/

/* Drop all non-system stored procs */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'P' AND category = 0 ORDER BY [name])

WHILE @name is not null
BEGIN
    SELECT @SQL = 'DROP PROCEDURE [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Procedure: ' + @name
    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'P' AND category = 0 AND [name] > @name ORDER BY [name])
END
GO

/* Drop all views */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'V' AND category = 0 ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP VIEW [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped View: ' + @name
    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'V' AND category = 0 AND [name] > @name ORDER BY [name])
END
GO

/* Drop all functions */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0 ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP FUNCTION [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Function: ' + @name
    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0 AND [name] > @name ORDER BY [name])
END
GO

/* Drop all Foreign Key constraints */
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)

WHILE @name is not null
BEGIN
    SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
    WHILE @constraint IS NOT NULL
    BEGIN
        SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
        EXEC (@SQL)
        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
    END
SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)
END
GO

/* Drop all Primary Key constraints */
DECLARE @name VARCHAR(128)
DECLARE @constraint VARCHAR(254)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY TABLE_NAME)

WHILE @name IS NOT NULL
BEGIN
    SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
    WHILE @constraint is not null
    BEGIN
        SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint)+']'
        EXEC (@SQL)
        PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
    END
SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY TABLE_NAME)
END
GO

/* Drop all tables */
DECLARE @name VARCHAR(128)
DECLARE @SQL VARCHAR(254)

SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 ORDER BY [name])

WHILE @name IS NOT NULL
BEGIN
    SELECT @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) +']'
    EXEC (@SQL)
    PRINT 'Dropped Table: ' + @name
    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] > @name ORDER BY [name])
END
GO


/********************************************************************************************************
 * CREATE TABLES
 ********************************************************************************************************
*/

CREATE TABLE [dbo].[Category] (
    [CategoryID]  INT           NOT NULL,
    [Name]        VARCHAR (80)  NULL,
    [Description] VARCHAR (255) NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);


CREATE TABLE [dbo].[Supplier] (
    [SupplierId] INT          NOT NULL,
    [Name]       VARCHAR (80) NOT NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([SupplierId] ASC)
);

CREATE TABLE [dbo].[User] (
    [Id]		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    [Username]	VARCHAR (64)	NOT NULL UNIQUE,
    [Password]	VARCHAR (255)	NOT NULL,
    [FirstName]	VARCHAR (80)	NOT NULL,
    [LastName]	VARCHAR (80)	NOT NULL,
    [Email]		VARCHAR (254)	NOT NULL UNIQUE,
	[Phone]		VARCHAR (20)	NOT NULL
    /* CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC) */
);


CREATE TABLE [dbo].[Orders] (
    [OrderID]   INT      IDENTITY (1, 1) NOT NULL,
    [UserID]    INT      NOT NULL,
    [OrderDate] DATETIME CONSTRAINT [DF_Orders_OrderDate] DEFAULT (getdate()) NOT NULL,
    [Status]    CHAR (1) CONSTRAINT [DF_Orders_StatusCode] DEFAULT ('P') NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Orders_Employee] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([Id])
);


CREATE TABLE [dbo].[Books] (
    [ISBN]       CHAR (10)       NOT NULL,
    [CategoryID] INT             NOT NULL,
    [Title]      VARCHAR (80)    NULL,
    [Author]     VARCHAR (255)   NULL,
    [Price]      DECIMAL (10, 2) NULL,
    [SupplierId] INT             NULL,
    [Year]       NCHAR (4)       NULL,
    [Edition]    NCHAR (2)       NOT NULL,
    [Publisher]  NVARCHAR (50)   NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ISBN] ASC),
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]),
    CONSTRAINT [FK_Product_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([SupplierId])
);



CREATE TABLE [dbo].[OrderItem] (
    [OrderID]  INT       NOT NULL,
    [ISBN]     CHAR (10) NOT NULL,
    [Quantity] INT       NOT NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED ([OrderID] ASC, [ISBN] ASC),
    CONSTRAINT [FK_OrderItem_Orders] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Orders] ([OrderID]),
    CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY ([ISBN]) REFERENCES [dbo].[Books] ([ISBN])
);

/********************************************************************************************************
 * SEED WITH FAKE DATA
 ********************************************************************************************************
*/

SET IDENTITY_INSERT [dbo].[User] ON
INSERT INTO [dbo].[User] ([Id], [UserName], [Password], [FirstName], [LastName], [Email], [Phone]) 
VALUES 
	(1, 'shaverz', 'zs1234', 'Zach', 'Shaver', 'shaverz@uwindsor.ca', '2656546565'),
	(2, 'edit_user', 'testpass1234', 'John', 'Smith', 'edit@user.com', '1231231234')
SET IDENTITY_INSERT [dbo].[User] OFF

INSERT INTO [dbo].[Category] ([CategoryID], [Name], [Description]) 
VALUES 
	(1, N'Programming Languages', N'Programming Language'),
	(2, N'Software Development', N'Project Management'),
	(3, N'New Books in Fall 2015', N'Fall 2015'),
	(4, N'Textbooks', N'Used in class')

INSERT INTO [dbo].[Supplier] ([SupplierId], [Name]) 
VALUES 
	(1, N'amazon.ca'),
	(2, N'Chapter Bookstore'),
	(3, N'Springer')

INSERT INTO [dbo].[Books] ([ISBN], [CategoryID], [Title], [Author], [Price], [SupplierId], [Year], [Edition], [Publisher]) 
VALUES 
	(N'0135974445', 2, N'Agile Software Development, Principles, Patterns, and Practices', N'Robert C. Martin', CAST(70.40 AS Decimal(10, 2)), 1, N'2002', N'1 ', N'Pearson'),
	(N'0321146530', 2, N'Test Driven Development: By Example', N'Kent Beck', CAST(41.59 AS Decimal(10, 2)), 1, N'2002', N'1 ', N'Addison-Wesley Professional'),
	(N'0321278658', 2, N'Extreme Programming Explained: Embrace Change', N'Kent Beck and Cynthia Andres', CAST(44.63 AS Decimal(10, 2)), 1, N'2004', N'2 ', N'Addison-Wesley Professional'),
	(N'073561993X', 2, N'Agile Project Management with Scrum', N'Ken Schwaber', CAST(26.45 AS Decimal(10, 2)), 1, N'2004', N'1 ', N'Microsoft Press'),
	(N'1118026241', 2, N'Agile Project Management For Dummies', N'Mark C. Layton', CAST(26.99 AS Decimal(10, 2)), 1, N'2012', N'1 ', N'For Dummies'),
	(N'1285096339', 1, N'Microsoft Visual C# 2012: An Introduction to Object-Oriented Programming', N'Joyce Farrell', CAST(185.11 AS Decimal(10, 2)), 1, N'2013', N'5 ', N'Course Technology'),
	(N'1491922834', 4, N'Learning Virtual Reality: Developing Immersive Experiences and Applications', N'Tony Parisi', CAST(39.71 AS Decimal(10, 2)), 1, N'2015', N'1 ', N'O''Reilly Media'),
	(N'1554683831', 3, N'The Illegal', N'Lawrence Hill', CAST(20.99 AS Decimal(10, 2)), 2, N'2015', N'  ', N'Harper Collins'),
	(N'1617290890', 1, N'The Art of Unit Testing: with examples in C#', N'Roy Osherove', CAST(57.28 AS Decimal(10, 2)), 1, N'2013', N'1 ', N'Manning Publications'),
	(N'161729134X', 1, N'NULLC# in Depth', N'Jon Skeet', CAST(41.22 AS Decimal(10, 2)), 1, N'2013', N'3 ', N'Manning Publications'),
	(N'161729232X', 1, N'Unity in Action: Multiplatform Game Development in C# with Unity 5', N'Joe Hocking', CAST(47.54 AS Decimal(10, 2)), 1, N'2015', N'1 ', N'Manning Publications'),
	(N'1852333944', 1, N'Essential Java 3D Fast : Developing 3D Graphics Applications in Java', N'Ian Palmer', CAST(89.99 AS Decimal(10, 2)), 3, N'2001', N'  ', N'Springer-Verlag'),
	(N'1884777902', 1, N'3D User Interfaces with Java 3D', N'Jon Barrilleaux', CAST(6.71 AS Decimal(10, 2)), 1, N'2000', N'  ', N'Manning Publications'),
	(N'1941393101', 4, N'Virtual Reality Beginner''s Guide + Google Cardboard', N'Patrick Buckley and Frederic Lardinois', CAST(20.95 AS Decimal(10, 2)), 1, N'2014', N'  ', N'Regan Arts'),
	(N'xxxxxxxxxx', 4, N'Unity Virtual Reality Projects', N'Jonathan Linowes', CAST(31.99 AS Decimal(10, 2)), 1, N'2015', N'1 ', N'Packt Publishing')

SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT INTO [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [Status]) 
VALUES 
	(1, 1, N'2015-09-18 09:23:58', N'P'),
	(4, 1, N'2015-09-18 10:03:00', N'P'),
	(7, 1, N'2015-09-18 22:25:26', N'P'),
	(8, 1, N'2015-09-18 22:29:01', N'P'),
	(9, 1, N'2015-09-18 22:31:00', N'P'),
	(10, 1, N'2018-10-18 20:32:20', N'P'),
	(11, 1, N'2018-10-18 20:39:00', N'P'),
	(12, 1, N'2018-10-18 20:49:02', N'P')
SET IDENTITY_INSERT [dbo].[Orders] OFF

INSERT INTO [dbo].[OrderItem] ([OrderID], [ISBN], [Quantity]) 
VALUES 
	(4, N'1617290890', 1),
	(4, N'161729232X', 2),
	(8, N'1852333944', 2),
	(9, N'1554683831', 1),
	(10, N'161729232X', 1),
	(11, N'1852333944', 3),
	(12, N'161729232X', 1)
