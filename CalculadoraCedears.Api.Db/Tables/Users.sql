CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [ThirdPartyUserId] VARCHAR(100) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [LastLogin] DATETIME NOT NULL,
    [RefreshToken] VARCHAR(50) NOT NULL,
    [ExpiresAt] DATETIME NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
)
