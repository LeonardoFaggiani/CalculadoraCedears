CREATE TABLE [dbo].[Cedears]
(
    [Id]                    UNIQUEIDENTIFIER,
    [Ticker]                VARCHAR(5)      NOT NULL,
    [Name]                  VARCHAR(50)     NOT NULL,
    [Ratio]                 INT             NOT NULL,
    CONSTRAINT [PK_Cedears] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_Cedears] ON [dbo].[Cedears]
(
    [Ticker]
)