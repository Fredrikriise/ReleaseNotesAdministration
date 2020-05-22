----------------------------------------------------
CREATE TABLE Products (
    [ProductId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProductName] varchar(max),
    [ProductImage] varchar(max)
);

----------------------------------------------------
CREATE TABLE WorkItems (
    [Id] [int] NOT NULL PRIMARY KEY,
    [Title] varchar(max),
    [AssignedTo] varchar(max),
    [State] varchar(50)
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
    [LastUpdateDate] datetime NOT NULL DEFAULT (GETUTCDATE()),
    [isDraft] [bit] NOT NULL,
    [PickedWorkItems] varchar(max)
);