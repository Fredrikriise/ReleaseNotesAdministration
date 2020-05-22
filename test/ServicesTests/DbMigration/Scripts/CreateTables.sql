----------------------------------------------------
CREATE TABLE Products (
    [ProductId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProductName] varchar(max),
    [ProductImage] varchar(max)
);

-----------------------------------------------------
CREATE TABLE ReleaseNotes (
    [Title] varchar(64) NOT NULL,
    [BodyText] varchar(max),
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProductId] int,
    [CreatedBy] varchar(max),
    [CreatedDate] datetime NOT NULL DEFAULT (GETUTCDATE()),
    [LastUpdatedBy] varchar(max),
    [LastUpdatedDate] datetime NOT NULL DEFAULT (GETUTCDATE()),
    [isDraft] [bit] NOT NULL,
    [PickedWorkItems] varchar(max)

    CONSTRAINT FK_ReleaseNotes_Products
    FOREIGN KEY (ProductId)
    REFERENCES Products (ProductId)
);

----------------------------------------------------
CREATE TABLE WorkItems (
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] varchar(max),
    [AssignedTo] varchar(max),
    [State] varchar(50)
);