ALTER TABLE [User] ADD [IsAdministrator] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [User] ADD [Password] nvarchar(max) NULL;

GO

CREATE TABLE [ItemCardImage] (
    [Id] int NOT NULL IDENTITY,
    [ItemCardId] int NOT NULL,
    [ImagePath] nvarchar(max) NULL,
    CONSTRAINT [PK_ItemCardImage] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ItemCardImage_ItemCard_ItemCardId] FOREIGN KEY ([ItemCardId]) REFERENCES [ItemCard] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_ItemCard_IsValid] ON [ItemCard] ([IsValid]);

GO

CREATE INDEX [IX_Category_IsValid] ON [Category] ([IsValid]);

GO

CREATE INDEX [IX_ItemCardImage_ItemCardId] ON [ItemCardImage] ([ItemCardId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200613162153_Add_ItemCardImage', N'3.1.4');

GO

