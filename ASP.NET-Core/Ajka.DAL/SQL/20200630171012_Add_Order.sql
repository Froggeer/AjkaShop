CREATE TABLE [Order] (
    [Id] int NOT NULL IDENTITY,
    [CreateDate] datetime2 NOT NULL,
    [Discount] decimal(12,2) NOT NULL,
    [CustomerEmail] nvarchar(max) NULL,
    [State] int NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [OrderItem] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ItemCardId] int NOT NULL,
    [OrderedQuantity] int NOT NULL,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItem_ItemCard_ItemCardId] FOREIGN KEY ([ItemCardId]) REFERENCES [ItemCard] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_OrderItem_ItemCardId] ON [OrderItem] ([ItemCardId]);

GO

CREATE INDEX [IX_OrderItem_OrderId] ON [OrderItem] ([OrderId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200630171012_Add_Order', N'3.1.5');

GO

