ALTER TABLE [OrderItem] ADD [ColorName] nvarchar(max) NULL;

GO

ALTER TABLE [OrderItem] ADD [SizeName] nvarchar(max) NULL;

GO

ALTER TABLE [ItemCardImage] ADD [ColorName] nvarchar(max) NULL;

GO

ALTER TABLE [ItemCard] ADD [SizeList] nvarchar(max) NULL;

GO

CREATE INDEX [IX_ItemCard_IsAdlerProduct] ON [ItemCard] ([IsAdlerProduct]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200708075354_Add_ColorSize', N'3.1.5');

GO

