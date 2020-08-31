DROP INDEX [IX_ItemCard_IsValid] ON [ItemCard];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ItemCard]') AND [c].[name] = N'IsValid');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ItemCard] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ItemCard] DROP COLUMN [IsValid];

GO

ALTER TABLE [ItemCard] ADD [Price] decimal(12,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [ItemCard] ADD [State] int NOT NULL DEFAULT 0;

GO

CREATE INDEX [IX_ItemCard_State] ON [ItemCard] ([State]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200625151258_Add_ItemCardsPrice', N'3.1.4');

GO

