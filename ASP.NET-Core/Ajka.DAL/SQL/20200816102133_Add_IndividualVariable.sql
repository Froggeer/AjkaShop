CREATE TABLE [IndividualVariable] (
    [Id] int NOT NULL IDENTITY,
    [KeyName] nvarchar(max) NOT NULL,
    [ValueString] nvarchar(max) NULL,
    CONSTRAINT [PK_IndividualVariable] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200816102133_Add_IndividualVariable', N'3.1.6');

GO

