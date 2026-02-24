CREATE DATABASE ComfortFurnitureDB;
GO

USE ComfortFurnitureDB;
GO

-- Справочник типов продукции
CREATE TABLE ProductTypes (
    ProductTypeId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Coefficient FLOAT NOT NULL        -- коэффициент типа продукции для расчёта сырья
);
GO

-- Справочник типов материалов
CREATE TABLE MaterialTypes (
    MaterialTypeId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    LossPercent FLOAT NOT NULL        -- процент потерь сырья
);
GO

-- Цеха
CREATE TABLE Workshops (
    WorkshopId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(50) NOT NULL,       -- например, 'Проектирование', 'Обработка', 'Сборка'
    EmployeeCount INT NOT NULL        -- количество человек для производства
);
GO

-- Основная таблица продукции
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductTypeId INT NOT NULL FOREIGN KEY REFERENCES ProductTypes(ProductTypeId),
    Name NVARCHAR(200) NOT NULL,
    Article NVARCHAR(50) NOT NULL UNIQUE,
    MinPriceForPartner DECIMAL(18,2) NOT NULL,
    MainMaterialId INT NOT NULL FOREIGN KEY REFERENCES MaterialTypes(MaterialTypeId),
    -- Опционально можно добавить Description, Image и др., но для начала достаточно
    -- Эти поля не обязательны по заданию, но могут пригодиться.
    Description NVARCHAR(MAX) NULL,
    Image VARBINARY(MAX) NULL
);
GO

-- Связь продукции с цехами (время изготовления в каждом цехе)
CREATE TABLE ProductWorkshops (
    ProductWorkshopId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(ProductId) ON DELETE CASCADE,
    WorkshopId INT NOT NULL FOREIGN KEY REFERENCES Workshops(WorkshopId) ON DELETE CASCADE,
    TimeHours FLOAT NOT NULL          -- время изготовления в этом цехе (часы)
);
GO

-- Индексы для ускорения
CREATE INDEX IX_Products_Article ON Products(Article);
CREATE INDEX IX_ProductWorkshops_ProductId ON ProductWorkshops(ProductId);