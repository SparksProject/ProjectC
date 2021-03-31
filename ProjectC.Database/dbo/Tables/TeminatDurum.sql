CREATE TABLE [dbo].[TeminatDurum] (
    [TeminatDurumId] INT           IDENTITY (1, 1) NOT NULL,
    [Adı]            VARCHAR (100) NULL,
    CONSTRAINT [PK_TeminatDurum] PRIMARY KEY CLUSTERED ([TeminatDurumId] ASC)
);

