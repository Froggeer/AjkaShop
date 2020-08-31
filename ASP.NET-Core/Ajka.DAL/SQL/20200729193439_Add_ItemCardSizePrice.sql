DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ItemCard]') AND [c].[name] = N'SizeList');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ItemCard] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ItemCard] DROP COLUMN [SizeList];

GO

ALTER TABLE [OrderItem] ADD [ImagePath] nvarchar(max) NULL;

GO

ALTER TABLE [OrderItem] ADD [ItemCardSizePriceId] int NULL;

GO

CREATE TABLE [ItemCardSizePrice] (
    [Id] int NOT NULL IDENTITY,
    [ItemCardId] int NOT NULL,
    [SizeName] nvarchar(max) NULL,
    [Price] decimal(12,2) NOT NULL,
    CONSTRAINT [PK_ItemCardSizePrice] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ItemCardSizePrice_ItemCard_ItemCardId] FOREIGN KEY ([ItemCardId]) REFERENCES [ItemCard] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_OrderItem_ItemCardSizePriceId] ON [OrderItem] ([ItemCardSizePriceId]);

GO

CREATE INDEX [IX_ItemCardSizePrice_ItemCardId] ON [ItemCardSizePrice] ([ItemCardId]);

GO

ALTER TABLE [OrderItem] ADD CONSTRAINT [FK_OrderItem_ItemCardSizePrice_ItemCardSizePriceId] FOREIGN KEY ([ItemCardSizePriceId]) REFERENCES [ItemCardSizePrice] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200729193439_Add_ItemCardSizePrice', N'3.1.6');

GO

