ALTER TABLE [Users] DROP CONSTRAINT [PK_Users];

GO

EXEC sp_rename N'[Users]', N'User';

GO

ALTER TABLE [User] ADD CONSTRAINT [PK_User] PRIMARY KEY ([Id]);

GO

CREATE INDEX [IX_User_IsValid] ON [User] ([IsValid]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200507210033_Change_UserTableName', N'3.1.3');

GO

