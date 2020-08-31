ALTER TABLE [Order] ADD [Note] nvarchar(max) NULL;

GO

CREATE TABLE [Invoice] (
    [Id] int NOT NULL IDENTITY,
    [InvoiceNumber] int NOT NULL,
    [RecipientName] nvarchar(max) NULL,
    [RecipientStreet] nvarchar(max) NULL,
    [RecipientCity] nvarchar(max) NULL,
    [RecipientZipCode] nvarchar(max) NULL,
    [VariableSymbol] nvarchar(max) NULL,
    [PaymentMethod] int NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [TaxablePerformanceDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [InvoiceItem] (
    [Id] int NOT NULL IDENTITY,
    [InvoiceId] int NOT NULL,
    [OrderNumber] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [PricePerPiece] decimal(12,2) NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_InvoiceItem] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InvoiceItem_Invoice_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoice] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Invoice_ReleaseDate] ON [Invoice] ([ReleaseDate]);

GO

CREATE INDEX [IX_InvoiceItem_InvoiceId] ON [InvoiceItem] ([InvoiceId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200715182931_Add_Invoice', N'3.1.5');

GO

