CREATE TABLE [dbo].[TeminatTipi] (
    [TeminatTipiId] INT           IDENTITY (1, 1) NOT NULL,
    [Adı]           VARCHAR (100) NULL,
    CONSTRAINT [PK_TeminatTipi] PRIMARY KEY CLUSTERED ([TeminatTipiId] ASC)
);

