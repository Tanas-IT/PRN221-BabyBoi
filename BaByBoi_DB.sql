Use [master]
GO
DROP DATABASE [BaByBoi]
Go
Create Database [BaByBoi]
GO
USE [BaByBoi]

CREATE TABLE [dbo].[Role](
    [RoleID] INT IDENTITY(1,1) PRIMARY KEY,
    [RoleName] NVARCHAR(50)
);

CREATE TABLE [dbo].[Size](
    [SizeID] INT IDENTITY(1,1) PRIMARY KEY,
    [SizeCode] INT,
    [SizeName] NVARCHAR(100),
    [Description] NVARCHAR(MAX)
);


CREATE TABLE [dbo].[Category](
    [CategoryID] INT IDENTITY(1,1) PRIMARY KEY,
    [CategoryCode] NVARCHAR(MAX),
    [CategoryName] NVARCHAR(MAX),
    [CreateDate] DATE,
    [UpdateDate] DATE,
    [UpdateBy] NVARCHAR(MAX),
    [CreateBy] NVARCHAR(MAX),
    [Status] INT
);

CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Email] [nvarchar](100) NOT NULL UNIQUE,
	[Password] [nvarchar](max) NULL,
	[Phone] [nvarchar](12) NULL,
	[FullName] [nvarchar](100) NULL,
	[CreateDate] [date] NULL,
	[UpdateDate] [date] NULL,
	[Status] [int] NULL,
	[BirthDate] [date] NULL,
	[Gender] [int] NULL,
	[RoleID] [int] NULL,
    CONSTRAINT FK_User_Role FOREIGN KEY (RoleID) REFERENCES [dbo].[Role](RoleID)
);


CREATE TABLE [dbo].[Payment](
    [PaymentID] INT IDENTITY(1,1) PRIMARY KEY,
    [PaymentCode] NVARCHAR(MAX),
    [PaymentName] NVARCHAR(MAX),
    [Status] INT
);

CREATE TABLE [dbo].[Product](
    [ProductID] INT IDENTITY(1,1) PRIMARY KEY,
    [ProductCode] NVARCHAR(36),
    [ProductName] NVARCHAR(MAX),
    [CreateDate] DATE,
    [CreateBy] NVARCHAR(MAX),
    [UpdateDate] DATE,
    [UpdateBy] NVARCHAR(MAX),
    [Description] NVARCHAR(MAX),
    [Status] INT,
    [CategoryID] INT,
    [Discount] INT,
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES [dbo].[Category](CategoryID)
);

CREATE TABLE [dbo].[ProductImage](
    [ImageID] INT IDENTITY(1,1) PRIMARY KEY,
    [ImageURL] NVARCHAR(MAX),
    [ImageName] NVARCHAR(MAX),
    [CreateBy] NVARCHAR(MAX),
    [ProductID] INT,
    [UpdateBy] NVARCHAR(MAX),
    CONSTRAINT FK_ProductImage_Product FOREIGN KEY (ProductID) REFERENCES [dbo].[Product](ProductID)
);

CREATE TABLE [dbo].[ProductSize](
    [Price] FLOAT,
    [SizeID] INT,
    [ProductID] INT,
    [Quantity] INT,
    PRIMARY KEY (SizeID, ProductID),
    CONSTRAINT FK_ProductSize_Product FOREIGN KEY (ProductID) REFERENCES [dbo].[Product](ProductID),
    CONSTRAINT FK_ProductSize_Size FOREIGN KEY (SizeID) REFERENCES [dbo].[Size](SizeID)
);

CREATE TABLE [dbo].[Voucher](
    [VoucherID] INT IDENTITY(1,1) PRIMARY KEY,
    [VoucherCode] NVARCHAR(MAX),
    [StartDate] DATE,
    [EndDate] DATE,
    [Type] NVARCHAR(100),
    [Percent] INT,
    [Value] INT,
    [TriggerValue] INT,
    [Status] INT
);

CREATE TABLE [dbo].[Order](
    [OrderID] INT IDENTITY(1,1) PRIMARY KEY,
    [OrderDate] DATE,
    [OrderCode] NVARCHAR(MAX),
    [UserID] INT,
    [Feedback] NVARCHAR(MAX),
    [TotalPrice] FLOAT,
    [TotalProduct] INT,
    [Rating] INT,
    [Address] NVARCHAR(MAX),
    [Status] INT,
    [Description] TEXT,
    [PaymentID] INT,
    [VoucherID] INT,
    CONSTRAINT FK_Order_User FOREIGN KEY (UserID) REFERENCES [dbo].[User](UserID),
    CONSTRAINT FK_Order_Payment FOREIGN KEY (PaymentID) REFERENCES [dbo].[Payment](PaymentID),
    CONSTRAINT FK_Order_Voucher FOREIGN KEY (VoucherID) REFERENCES [dbo].[Voucher](VoucherID)
);

CREATE TABLE [dbo].[OrderDetail](
    [OrderID] INT,
    [ProductID] INT,
    [Quantity] INT,
    [Price] FLOAT,
    [SizeID] INT,
    PRIMARY KEY (OrderID, ProductID, SizeID),
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderID) REFERENCES [dbo].[Order](OrderID),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductID) REFERENCES [dbo].[Product](ProductID),
    CONSTRAINT FK_OrderDetail_ProductSize FOREIGN KEY (SizeID, ProductID) REFERENCES [dbo].[ProductSize](SizeID, ProductID)
);

-- Insert data into [Role]
INSERT INTO [dbo].[Role] ([RoleName])
VALUES 
('Admin'), 
('Customer');

-- Insert data into User
INSERT INTO [dbo].[User] ( [Email], [Password], [Phone], [FullName], [CreateDate], [UpdateDate], [Status], [BirthDate], [Gender], [RoleID])
VALUES 
('admin@example.com', 'password123', '1234567890', 'Admin User', '2024-01-01', '2024-01-01', 1, '1980-01-01', 1, 1),
('customer1@example.com', 'password123', '0987654321', 'Customer One', '2024-01-01', '2024-01-01', 1, '1990-01-01', 1, 2),
( 'customer2@example.com', 'password123', '1122334455', 'Customer Two', '2024-01-01', '2024-01-01', 1, '1992-02-02', 2, 2);

-- Insert data into [Category]
INSERT INTO [dbo].[Category] ([CategoryCode], [CategoryName], [CreateDate], [UpdateDate], [UpdateBy], [CreateBy], [Status])
VALUES 
('CAT001', N'Tả quần', '2024-01-01', '2024-01-01', 'admin', 'admin', 1),
('CAT002', N'Tả dán', '2024-01-01', '2024-01-01', 'admin', 'admin', 1),
('CAT003', N'Miếng lót', '2024-01-01', '2024-01-01', 'admin', 'admin', 1);

-- Insert data into [Payment]
INSERT INTO [dbo].[Payment] ([PaymentCode], [PaymentName], [Status])
VALUES 
('PAY001', 'Credit Card', 1),
('PAY002', 'PayPal', 1),
('PAY003', 'Cash on Delivery', 1);

-- Insert data into [Voucher]
INSERT INTO [dbo].[Voucher] ([VoucherCode], [StartDate], [EndDate], [Type], [Percent], [Value], [TriggerValue], [Status])
VALUES 
('VOUCHER10', '2024-01-01', '2024-12-31', 'Discount', 10, NULL, 100, 1),
('VOUCHER20', '2024-01-01', '2024-12-31', 'Discount', 20, NULL, 200, 1),
('VOUCHER30', '2024-01-01', '2024-12-31', 'Discount', 30, NULL, 300, 1);
GO
-- Insert data into [Product]
INSERT INTO [dbo].[Product] ([ProductCode], [ProductName], [CreateDate], [CreateBy], [UpdateDate], [UpdateBy], [Description], [Status], [CategoryID], [Discount])
VALUES 
('PROD001', N'Bỉm tã quần Molfix thiên nhiên (XXXL, 20-35kg, 22 miếng) + 6 miếng', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿Với thành phần sợi tre tự nhiên và bông Organic, Molfix Thiên Nhiên là dòng sản phẩm đột phá từ nhà sản xuất tã trẻ em lớn thứ 5 thế giới dành riêng cho thị trường Việt Nam, đồng thời được công ty mẹ Hayat Việt Nam đầu tư dây chuyền sản xuất ngay tại Bình Phước để mang tới dòng tã chất lượng quốc tế, giá thành nội địa đáp ứng nhu cầu của các gia đình Việt.', 1, 1, 10),
('PROD002', N'Bỉm tã quần Huggies Platinum Nature Made L size 44 miếng (9-14kg)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿Tã Huggies Platinum Nature Made size L phù hợp với bé từ 9-14kg. Đây dòng tã mới nhất hiện nay của Huggies với 100% nguyên liệu sợi tự nhiên được nhập khẩu từ châu Âu, an toàn với làn da mỏng manh của con yêu.', 1, 1, 5),
('PROD003', N'Tã quần Huggies Skincare gói cực đại (XXL, >15kg, 54 miếng)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿Tã quần Huggies Skincare size XXL phù hợp với bé trên 15 kg. Sản phẩm bổ sung tinh chất tràm trà tự nhiên, giúp mang đến cảm giác thoải mái, dễ chịu và ngừa hăm tã cho bé. ', 1, 1, 15),

('PROD004', N'Combo 2 bỉm tã dán Huggies Platinum Nature Made size Newborn 60 miếng (dưới 5kg)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿Tã Huggies Platinum Nature Made size NB phù hợp với bé dưới 5kg. Đây dòng tã mới nhất hiện nay của Huggies với 100% nguyên liệu sợi tự nhiên được nhập khẩu từ châu Âu, an toàn với làn da mỏng manh của con yêu.', 1, 2, 15),
('PROD004', N'Conbo 2 Tã dán GOO.N Mommy Kiss (M,7kg-12kg, 56 miếng)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿Conbo 2 Tã dán GOO.N Mommy Kiss (M,7kg-12kg, 56 miếng)', 1, 2, 15),
('PROD004', N'Bỉm tã dán Elprairie AW (S, dưới 6.5kg, 34 miếng)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿Tã dán Elprairie size S là sản phẩm phù hợp với bé có cân nặng dưới 6.5kg. Được làm từ chất liệu an toàn, kết hợp cùng thiết kế thông minh, sản phẩm giúp mang đến cho bé cảm giác khô thoáng, dễ chịu suốt cả ngày dài.', 1, 2, 15),

('PROD004', N'Miếng lót Bobby size Newborn 1 108 miếng (dưới 5kg)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿Miếng lót Bobby size Newborn 1 ﻿108 miếng là sản phẩm dành cho bé sơ sinh dưới 1 tháng tuổi thuộc thương hiệu Bobby đến từ tập đoàn Unicharm Nhật Bản. Miếng lót sơ sinh Bobby Newborn đột phá với giải pháp chống hăm toàn diện cùng khả năng thấm hút vượt trội.', 1, 3, 15),
('PROD004', N'Miếng lót Molfix Thiên Nhiên (Newborn 1, < 1 tháng, 90 miếng + 10 miếng)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿Miếng lót Molfix Thiên Nhiên (Newborn 1, < 1 tháng, 90 miếng + 10 miếng) là sản phẩm dành dành cho trẻ sơ sinh dưới 1 tháng tuổi. Với thiết kế rãnh rốn êm ái, miếng lót giúp bảo vệ tối đa vùng rốn của bé yêu. Ba mẹ càng yên tâm hơn khi Molfix Thiên Nhiên có thành phần chính là sợi tre tự nhiên và bông Organic.', 1, 3, 15),
('PROD004', N'Miếng lót lọt lòng Huggies Skin Perfect (Newborn 1, dưới 5kg, 108 miếng)', '2024-01-01', 'admin', '2024-01-01', 'admin', N'﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿Miếng lót Huggies Skin Perfect phù hợp với bé dưới 5kg.Sản phẩm được làm từ chất liệu êm mềm và có khả năng thấm hút xuyên đêm đến 12h, cho bé yên giấc trọn đêm.', 1, 3, 15);
GO

-- Insert data into [ProductImage]
INSERT INTO [dbo].[ProductImage] ([ImageURL], [ImageName], [CreateBy], [ProductID], [UpdateBy])
VALUES 
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fbim-ta-quan-molfix-thien-nhien-xxxl-20-35kg-22-mieng-6-mieng.jpg?alt=media&token=75b50225-553a-4f71-9989-190c32ccf1a0', N'Bỉm tã quần Molfix thiên nhiên (XXXL, 20-35kg, 22 miếng) + 6 miếng', 'admin', 1, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fta-quan-sieu-cao-cap-huggies-platinum-nature-made-l-44-mieng.jpg?alt=media&token=26bbad18-313e-4489-ac21-c22aea310520', N'Bỉm tã quần Huggies Platinum Nature Made L size 44 miếng (9-14kg)', 'admin', 2, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fta-quan-huggies-skincare-goi-cuc-dai-xxl-15kg-54-mieng.png?alt=media&token=3235f602-fc22-441b-93e9-39a5762fce30', N'Tã quần Huggies Skincare gói cực đại (XXL, >15kg, 54 miếng)', 'admin', 3, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fbim-ta-dan-huggies-platinum-nature-made-size-newborn-60-mieng-duoi-5kg.jpg?alt=media&token=1d54ddfd-4983-423c-a325-57dc146fd6ad', N'Combo 2 bỉm tã dán Huggies Platinum Nature Made size Newborn 60 miếng (dưới 5kg)', 'admin', 4, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fta-dan-goo-n-mommy-kiss-s-4kg-8kg-62-mieng.jpg?alt=media&token=83376421-972a-43e4-afc8-9911563b3749', N'Conbo 2 Tã dán GOO.N Mommy Kiss (M,7kg-12kg, 56 miếng', 'admin', 5, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fbim-ta-dan-elprairie-aw-s-6-5kg-34-mieng.jpg?alt=media&token=d1fbcbe0-db95-4f75-a9f5-b1e464fdf330', N'Bỉm tã dán Elprairie AW (S, dưới 6.5kg, 34 miếng)', 'admin', 6, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fmieng-lot-so-sinh-bobby-fresh-newborn-1-duoi-5-kg-108-mieng.png?alt=media&token=2d177f30-8562-452f-9226-bbe9bdffdeee',  N'Miếng lót Bobby size Newborn 1 108 miếng (dưới 5kg)', 'admin', 7, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fmieng-lot-molfix-thien-nhien-newborn-1-1-thang-90-mieng.png?alt=media&token=e8bddfd6-f6bd-4c57-b50f-fae9e87b87f7', N'Miếng lót Molfix Thiên Nhiên (Newborn 1, < 1 tháng, 90 miếng + 10 miếng)', 'admin', 8, 'admin'),
('https://firebasestorage.googleapis.com/v0/b/babyboi-d872c.appspot.com/o/product%2Fmieng-lot-lot-long-huggies-skin-perfect-newborn-1-duoi-5kg-108-mieng.png?alt=media&token=349379b9-41a8-4ce0-a48f-e8eef766e94b',N'Miếng lót lọt lòng Huggies Skin Perfect (Newborn 1, dưới 5kg, 108 miếng)', 'admin', 9, 'admin');

-- Insert data into [Size]
INSERT INTO [dbo].[Size] ([SizeCode], [SizeName], [Description])
VALUES 
(1, 'Small', 'Small Size'),
(2, 'Medium', 'Medium Size'),
(3, 'Large', 'Large Size');

-- Insert data into [ProductSize]
INSERT INTO [dbo].[ProductSize] ([Price], [SizeID], [ProductID], [Quantity])
VALUES 
(205.00, 3, 1, 999),
(195.00, 2, 1, 999),
(185.00, 1, 1, 999),


(285.00, 3, 2, 999),
(275.00, 2, 2, 999),
(265.00, 1, 2, 999),

(319.00, 3, 3, 999),
(310.00, 2, 3, 999),
(300.00, 1, 3, 999),

(239.00, 1, 4, 999),
(249.00, 2, 4, 999),
(259.00, 3, 4, 999),

(325.00, 1, 5, 999),
(335.00, 2, 5, 999),
(345.00, 3, 5, 999),

(92.50, 1, 6, 999),
(93.50, 2, 6, 999),
(94.50, 3, 6, 999),

(125.00, 1, 7, 999),
(135.00, 2, 7, 999),
(145.00, 3, 7, 999),

(115.00, 1, 8, 999),
(125.00, 2, 8, 999),
(135.00, 3, 8, 999),

(130.50, 1, 9, 999),
(140.50, 2, 9, 999),
(150.50, 3, 9, 999);

-- Insert data into [Order]
INSERT INTO [dbo].[Order] ([OrderDate], [OrderCode], [UserID], [Feedback], [TotalPrice], [TotalProduct], [Rating], [Address], [Status], [Description], [PaymentID], [VoucherID])
VALUES 
('2024-01-01', 'ORD001', 2, 'Great service!', 300.50, 3, 5, '123 Customer St.', 1, 'N/A', 1, 1),
('2024-01-02', 'ORD002', 2, 'Quick delivery!', 150.75, 1, 4, '456 Customer Ave.', 1, 'N/A', 1, 1),
('2024-01-03', 'ORD003', 3, 'Good quality!', 200.00, 2, 5, '789 Customer Blvd.', 1, 'N/A', 2, 2);

-- Insert data into [OrderDetail]
INSERT INTO [dbo].[OrderDetail] ([OrderID], [ProductID], [Quantity], [Price], [SizeID])
VALUES 
(1, 1, 1, 299.99, 1),
(1, 1, 1, 19.99, 2),
(1, 3, 1, 9.99, 3),
(2, 2, 1, 299.99, 2),
(3, 3, 1, 19.99, 3);