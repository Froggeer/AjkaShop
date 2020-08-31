CREATE TABLE [ErrorLog] (
    [Id] int NOT NULL IDENTITY,
    [CreateDate] datetime2 NOT NULL,
    [Message] nvarchar(max) NULL,
    [FullDescription] nvarchar(max) NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200629225307_Add_ErrorLog', N'3.1.5');

GO
