CREATE TABLE [dbo].[Cedears]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR(50) NOT NULL,
    [Ticker] VARCHAR(5) NOT NULL,
    CONSTRAINT [PK_Sample] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Sample_Ticker] ON [dbo].[Cedears]
(
	[Ticker] ASC
)