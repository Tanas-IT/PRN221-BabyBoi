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
    [UserID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserName] NVARCHAR(100),
    [Password] NVARCHAR(MAX),
    [Email] NVARCHAR(100),
    [Phone] NVARCHAR(12),
    [FullName] NVARCHAR(100),
    [CreateDate] DATE,
    [UpdateDate] DATE,
    [Status] INT,
    [BirthDate] DATE,
    [Gender] INT,
    [RoleID] INT,
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

-- Insert data into [User]
INSERT INTO [dbo].[User] ([UserName], [Password], [Email], [Phone], [FullName], [CreateDate], [UpdateDate], [Status], [BirthDate], [Gender], [RoleID])
VALUES 
('admin', 'password123', 'admin@example.com', '1234567890', 'Admin User', '2024-01-01', '2024-01-01', 1, '1980-01-01', 1, 1),
('customer1', 'password123', 'customer1@example.com', '0987654321', 'Customer One', '2024-01-01', '2024-01-01', 1, '1990-01-01', 1, 2),
('customer2', 'password123', 'customer2@example.com', '1122334455', 'Customer Two', '2024-01-01', '2024-01-01', 1, '1992-02-02', 2, 2);

-- Insert data into [Category]
INSERT INTO [dbo].[Category] ([CategoryCode], [CategoryName], [CreateDate], [UpdateDate], [UpdateBy], [CreateBy], [Status])
VALUES 
('CAT001', 'Electronics', '2024-01-01', '2024-01-01', 'admin', 'admin', 1),
('CAT002', 'Clothing', '2024-01-01', '2024-01-01', 'admin', 'admin', 1),
('CAT003', 'Toys', '2024-01-01', '2024-01-01', 'admin', 'admin', 1);

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

-- Insert data into [Product]
INSERT INTO [dbo].[Product] ([ProductCode], [ProductName], [CreateDate], [CreateBy], [UpdateDate], [UpdateBy], [Description], [Status], [CategoryID], [Discount])
VALUES 
('PROD001', 'Smartphone', '2024-01-01', 'admin', '2024-01-01', 'admin', 'Latest model smartphone', 1, 1, 10),
('PROD002', 'T-shirt', '2024-01-01', 'admin', '2024-01-01', 'admin', 'Comfortable cotton t-shirt', 1, 2, 5),
('PROD003', 'Action Figure', '2024-01-01', 'admin', '2024-01-01', 'admin', 'Superhero action figure', 1, 3, 15);

-- Insert data into [ProductImage]
INSERT INTO [dbo].[ProductImage] ([ImageURL], [ImageName], [CreateBy], [ProductID], [UpdateBy])
VALUES 
('https://example.com/images/smartphone.jpg', 'Smartphone Image', 'admin', 1, 'admin'),
('https://example.com/images/tshirt.jpg', 'T-shirt Image', 'admin', 2, 'admin'),
('https://example.com/images/actionfigure.jpg', 'Action Figure Image', 'admin', 3, 'admin');

-- Insert data into [Size]
INSERT INTO [dbo].[Size] ([SizeCode], [SizeName], [Description])
VALUES 
(1, 'Small', 'Small Size'),
(2, 'Medium', 'Medium Size'),
(3, 'Large', 'Large Size');

-- Insert data into [ProductSize]
INSERT INTO [dbo].[ProductSize] ([Price], [SizeID], [ProductID], [Quantity])
VALUES 
(299.99, 1, 1, 50),
(299.99, 2, 1, 50),
(299.99, 3, 1, 50),
(19.99, 2, 2, 100),
(9.99, 3, 3, 200);

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
