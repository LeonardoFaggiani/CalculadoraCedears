CREATE TABLE [dbo].[Brokers]
(
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (25)       NOT NULL,
    [Comision]  DECIMAL(5,2)        NOT NULL
    CONSTRAINT [PK_Brokers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_Brokers] ON [dbo].[Brokers]
(
    [Name]
)