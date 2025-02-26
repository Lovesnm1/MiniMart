USE [master]
GO
/****** Object:  Database [MiniMart]    Script Date: 4/7/2024 5:17:32 PM ******/
CREATE DATABASE [MiniMart]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MiniMart', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.ANHNGO\MSSQL\DATA\MiniMart.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MiniMart_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.ANHNGO\MSSQL\DATA\MiniMart_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MiniMart] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MiniMart].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MiniMart] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MiniMart] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MiniMart] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MiniMart] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MiniMart] SET ARITHABORT OFF 
GO
ALTER DATABASE [MiniMart] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MiniMart] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MiniMart] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MiniMart] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MiniMart] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MiniMart] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MiniMart] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MiniMart] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MiniMart] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MiniMart] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MiniMart] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MiniMart] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MiniMart] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MiniMart] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MiniMart] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MiniMart] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MiniMart] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MiniMart] SET RECOVERY FULL 
GO
ALTER DATABASE [MiniMart] SET  MULTI_USER 
GO
ALTER DATABASE [MiniMart] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MiniMart] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MiniMart] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MiniMart] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MiniMart] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MiniMart] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MiniMart', N'ON'
GO
ALTER DATABASE [MiniMart] SET QUERY_STORE = ON
GO
ALTER DATABASE [MiniMart] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MiniMart]
GO
/****** Object:  Table [dbo].[Goods]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goods](
	[ID] [nchar](10) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Price] [float] NULL,
	[Brand] [nvarchar](50) NULL,
	[Catagory] [nvarchar](50) NULL,
	[MFG] [date] NULL,
	[EXP] [date] NULL,
	[Unit] [nvarchar](50) NULL,
	[InventoryNumber] [int] NULL,
	[ShipmentNumber] [int] NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_Goods] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoodsReceived]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoodsReceived](
	[Code] [nchar](10) NOT NULL,
	[ID_Goods] [nchar](10) NOT NULL,
	[CreatedDate] [date] NULL,
	[Price] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[Total] [float] NULL,
	[Note] [ntext] NULL,
	[Seller] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[Phone] [int] NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_GoodsReceived_1] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[ID_Goods] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[ID] [nchar](10) NOT NULL,
	[CreatedDate] [date] NULL,
	[CustomerName] [nvarchar](50) NULL,
	[ID_Membership] [nchar](10) NULL,
	[ID_Cashier] [nchar](10) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetails](
	[ID_Goods] [nchar](10) NOT NULL,
	[ID_Invoice] [nchar](10) NOT NULL,
	[Quantity] [int] NULL,
	[Total] [float] NULL,
 CONSTRAINT [PK_InvoiceDetails] PRIMARY KEY CLUSTERED 
(
	[ID_Goods] ASC,
	[ID_Invoice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Membership]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Membership](
	[ID] [nchar](10) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[Phone] [nchar](10) NULL,
	[Points] [int] NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_Membership] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shift]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shift](
	[Shift] [nchar](10) NOT NULL,
	[Counter] [char](10) NOT NULL,
	[Date] [date] NOT NULL,
	[ID_Cashier] [nchar](10) NULL,
 CONSTRAINT [PK_Shift_1] PRIMARY KEY CLUSTERED 
(
	[Shift] ASC,
	[Counter] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 4/7/2024 5:17:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [nchar](10) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nchar](10) NULL,
	[BasicSalary] [int] NULL,
	[Allowance] [int] NULL,
	[Bonus] [int] NULL,
	[Role] [nchar](50) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'A123      ', N'Toonies Spicy', 5000, N'Orion', N'Snack', CAST(N'2024-03-14' AS Date), CAST(N'2025-01-09' AS Date), N'Pack', 156, 6, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'B123      ', N'Khoai Tây Sườn', 5000, N'PepsiCo', N'Snack', CAST(N'2024-02-29' AS Date), CAST(N'2025-12-15' AS Date), N'Pack', 200, 20, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'C123      ', N'Pepsi', 8000, N'PepsiCo', N'Drink', CAST(N'2024-03-31' AS Date), CAST(N'2025-08-29' AS Date), N'Can', 184, 8, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'D123      ', N'C2', 7000, N'URC', N'Drink', CAST(N'2024-03-13' AS Date), CAST(N'2025-01-01' AS Date), N'Bottle', 109, NULL, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'E123      ', N'Snack Bí Đỏ', 6000, N'Oishi', N'Snack', CAST(N'2024-03-14' AS Date), CAST(N'2025-01-03' AS Date), N'Pack', 200, NULL, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'F123      ', N'Snack Cà Chua', 7000, N'Oishi', N'Snack', CAST(N'2024-02-08' AS Date), CAST(N'2025-02-06' AS Date), N'Pack', 180, NULL, 0)
INSERT [dbo].[Goods] ([ID], [Name], [Price], [Brand], [Catagory], [MFG], [EXP], [Unit], [InventoryNumber], [ShipmentNumber], [Hide]) VALUES (N'G123      ', N'Ô Long TEA +', 9000, N'Suntory', N'Drink', CAST(N'2024-02-01' AS Date), CAST(N'2025-02-01' AS Date), N'Bottle', 150, NULL, 0)
GO
INSERT [dbo].[GoodsReceived] ([Code], [ID_Goods], [CreatedDate], [Price], [Unit], [Quantity], [Total], [Note], [Seller], [Address], [Phone], [Hide]) VALUES (N'1         ', N'A123      ', CAST(N'2024-03-04' AS Date), 6000, N'pack', 100, 600000, NULL, N'OrionVN', N'TPHCM', 1900866874, 0)
INSERT [dbo].[GoodsReceived] ([Code], [ID_Goods], [CreatedDate], [Price], [Unit], [Quantity], [Total], [Note], [Seller], [Address], [Phone], [Hide]) VALUES (N'1         ', N'B123      ', CAST(N'2024-02-04' AS Date), 7000, N'pack', 200, 1400000, NULL, N'PepsiCo', N'TPHCM', 191301849, 0)
INSERT [dbo].[GoodsReceived] ([Code], [ID_Goods], [CreatedDate], [Price], [Unit], [Quantity], [Total], [Note], [Seller], [Address], [Phone], [Hide]) VALUES (N'2         ', N'A123      ', NULL, 9000, N'Pack', 8, NULL, N'', N'PepsiCo', N'TPHCM', 173138183, 0)
INSERT [dbo].[GoodsReceived] ([Code], [ID_Goods], [CreatedDate], [Price], [Unit], [Quantity], [Total], [Note], [Seller], [Address], [Phone], [Hide]) VALUES (N'3         ', N'A123      ', NULL, 9000, N'Pack', 8, NULL, N'', N'A', N'TPHCM', 817313131, 0)
INSERT [dbo].[GoodsReceived] ([Code], [ID_Goods], [CreatedDate], [Price], [Unit], [Quantity], [Total], [Note], [Seller], [Address], [Phone], [Hide]) VALUES (N'4         ', N'A123      ', NULL, 9000, N'pack', 8, NULL, N'', N'A', N'TPHCM', 813813813, 0)
GO
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2931      ', CAST(N'2024-03-29' AS Date), NULL, NULL, NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2932      ', CAST(N'2024-03-30' AS Date), NULL, NULL, NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2933      ', CAST(N'2024-04-03' AS Date), NULL, NULL, NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2934      ', CAST(N'2024-04-03' AS Date), NULL, NULL, NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2935      ', CAST(N'2024-04-03' AS Date), NULL, N'K999      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2936      ', CAST(N'2024-04-03' AS Date), NULL, N'T140      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2937      ', CAST(N'2024-04-03' AS Date), NULL, N'A056      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2938      ', CAST(N'2024-04-04' AS Date), NULL, N'A056      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2939      ', CAST(N'2024-04-04' AS Date), NULL, N'K999      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2940      ', CAST(N'2024-04-04' AS Date), NULL, NULL, NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2941      ', CAST(N'2024-04-04' AS Date), NULL, N'A056      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2942      ', CAST(N'2024-04-04' AS Date), NULL, N'K999      ', NULL, 0)
INSERT [dbo].[Invoice] ([ID], [CreatedDate], [CustomerName], [ID_Membership], [ID_Cashier], [Hide]) VALUES (N'2943      ', CAST(N'2024-04-04' AS Date), NULL, NULL, NULL, 0)
GO
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2931      ', 2, 10000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2932      ', 5, 25000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2933      ', 10, 50000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2935      ', 20, 100000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2936      ', 5, 25000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2938      ', 10, 50000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2939      ', 1, 5000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'A123      ', N'2941      ', 5, 25000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'B123      ', N'2931      ', 3, 15000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'B123      ', N'2934      ', 30, 150000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'B123      ', N'2937      ', 4, 20000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'B123      ', N'2940      ', 20, 100000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2931      ', 5, 40000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2932      ', 4, 32000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2933      ', 20, 160000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2935      ', 20, 160000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2936      ', 2, 16000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2942      ', 8, 64000)
INSERT [dbo].[InvoiceDetails] ([ID_Goods], [ID_Invoice], [Quantity], [Total]) VALUES (N'C123      ', N'2943      ', 8, 64000)
GO
INSERT [dbo].[Membership] ([ID], [Name], [Address], [Phone], [Points], [Hide]) VALUES (N'A056      ', N'Nguyen Viet Anh', N'Quan 6', N'0938354684', 25000, 0)
INSERT [dbo].[Membership] ([ID], [Name], [Address], [Phone], [Points], [Hide]) VALUES (N'H187      ', N'Nguyen Bao Hoang', N'Go Vap', N'0934818810', 0, 0)
INSERT [dbo].[Membership] ([ID], [Name], [Address], [Phone], [Points], [Hide]) VALUES (N'K999      ', N'Pham Nhat Kiet', N'Bình Chánh', N'0983718471', 69000, 0)
INSERT [dbo].[Membership] ([ID], [Name], [Address], [Phone], [Points], [Hide]) VALUES (N'T140      ', N'Dang Tien', N'Long An', N'0938354698', 1000, 0)
GO
INSERT [dbo].[Shift] ([Shift], [Counter], [Date], [ID_Cashier]) VALUES (N'Ca 1      ', N'Quay 1    ', CAST(N'2024-03-28' AS Date), NULL)
INSERT [dbo].[Shift] ([Shift], [Counter], [Date], [ID_Cashier]) VALUES (N'Ca 1      ', N'Quay 2    ', CAST(N'2024-03-15' AS Date), N'214021481 ')
INSERT [dbo].[Shift] ([Shift], [Counter], [Date], [ID_Cashier]) VALUES (N'Ca 1      ', N'Quay 2    ', CAST(N'2024-03-28' AS Date), N'214021481 ')
GO
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'214021481 ', N'Vu Tien Phat', N'0918118181', 1000000, 10000, 0, N'Cashier                                           ', 0)
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'215052056 ', N'Ngo Nguyen Viet Anh', N'0938354684', 0, 0, 0, N'Owner                                             ', 0)
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'215052125 ', N'Nguyen Anh', N'0891017171', 5000000, 20000, 1000000, N'HR Manager                                        ', 0)
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'215052140 ', N'Dang Ngoc Lan Tien', N'0971718116', 1500000, 100000, 1000000, N'Accountant                                        ', 0)
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'215052143 ', N'Bui Huy Hoang', N'0819101718', 8000000, 80000, 500000, N'Cashier                                           ', 0)
INSERT [dbo].[Staff] ([ID], [Name], [Phone], [BasicSalary], [Allowance], [Bonus], [Role], [Hide]) VALUES (N'215952859 ', N'Pham Gia Kiet', N'0902905361', 1000000, 0, 0, N'Storage Manager                                   ', 0)
GO
ALTER TABLE [dbo].[GoodsReceived]  WITH CHECK ADD  CONSTRAINT [FK_GoodsReceived_Goods] FOREIGN KEY([ID_Goods])
REFERENCES [dbo].[Goods] ([ID])
GO
ALTER TABLE [dbo].[GoodsReceived] CHECK CONSTRAINT [FK_GoodsReceived_Goods]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Membership] FOREIGN KEY([ID_Membership])
REFERENCES [dbo].[Membership] ([ID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Membership]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Staff] FOREIGN KEY([ID_Cashier])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Staff]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Goods] FOREIGN KEY([ID_Goods])
REFERENCES [dbo].[Goods] ([ID])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Goods]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Invoice] FOREIGN KEY([ID_Invoice])
REFERENCES [dbo].[Invoice] ([ID])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Invoice]
GO
ALTER TABLE [dbo].[Shift]  WITH CHECK ADD  CONSTRAINT [FK_Shift_Staff] FOREIGN KEY([ID_Cashier])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[Shift] CHECK CONSTRAINT [FK_Shift_Staff]
GO
USE [master]
GO
ALTER DATABASE [MiniMart] SET  READ_WRITE 
GO
