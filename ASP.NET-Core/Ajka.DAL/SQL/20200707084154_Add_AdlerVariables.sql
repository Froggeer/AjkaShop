ALTER TABLE [ItemCard] ADD [CommodityIdentifier] nvarchar(max) NULL;

GO

ALTER TABLE [ItemCard] ADD [IsAdlerProduct] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Category] ADD [GroupIdentifier] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200707084154_Add_AdlerVariables', N'3.1.5');

GO

