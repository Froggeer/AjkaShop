ALTER TABLE [User] ADD [Email] nvarchar(max) NULL;

GO

CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    [IsValid] bit NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ItemCard] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [Headline] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [ThumbnailImagePath] nvarchar(max) NULL,
    [Quantity] int NOT NULL,
    [IsValid] bit NOT NULL,
    CONSTRAINT [PK_ItemCard] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ItemCard_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_ItemCard_CategoryId] ON [ItemCard] ([CategoryId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200517093014_Add_CategoryAndGoodsItem', N'3.1.4');

GO

