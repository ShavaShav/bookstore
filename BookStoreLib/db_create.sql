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
    [Id]       INT           NOT NULL,
    [UserName] VARCHAR (20)  NOT NULL,
    [Password] VARCHAR (25)  NOT NULL,
    [Type]     CHAR (2)      NOT NULL,
    [Manager]  BIT           NOT NULL,
    [FullName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Orders] (
    [OrderID]   INT      IDENTITY (1, 1) NOT NULL,
    [UserID]    INT      NOT NULL,
    [OrderDate] DATETIME CONSTRAINT [DF_Orders_OrderDate] DEFAULT (getdate()) NOT NULL,
    [Status]    CHAR (1) CONSTRAINT [DF_Orders_StatusCode] DEFAULT ('P') NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Orders_Employee] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([Id])
);


CREATE TABLE [dbo].[Book] (
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
    CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY ([ISBN]) REFERENCES [dbo].[Book] ([ISBN])
);

/********************************************************************************************************
 * SEED WITH FAKE DATA
 ********************************************************************************************************
*/

INSERT INTO [dbo].[User] (Id, UserName, Password, Type, Manager, FullName)
VALUES(0, 'shaverz', 'zs1234', 'AD', '0', 'Zach Shaver');

