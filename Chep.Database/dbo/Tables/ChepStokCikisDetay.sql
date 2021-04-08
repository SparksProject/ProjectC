CREATE TABLE [dbo].[ChepStokCikisDetay] (
    [StokCikisDetayId] INT IDENTITY (1, 1) NOT NULL,
    [StokCikisId]      INT NOT NULL,
    [StokGirisDetayId] INT NOT NULL,
    [Miktar]           INT NULL,
    [Kg]               INT NULL,
    CONSTRAINT [PK__ChepStok__7A5C97E41CA2F026] PRIMARY KEY CLUSTERED ([StokCikisDetayId] ASC),
    CONSTRAINT [FK_ChepStokCikisDetay_ChepStokCikis] FOREIGN KEY ([StokCikisId]) REFERENCES [dbo].[ChepStokCikis] ([StokCikisId]),
    CONSTRAINT [FK_ChepStokCikisDetay_ChepStokGirisDetay] FOREIGN KEY ([StokGirisDetayId]) REFERENCES [dbo].[ChepStokGirisDetay] ([StokGirisDetayId])
);



