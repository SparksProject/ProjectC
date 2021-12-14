CREATE TABLE [dbo].[ChepStokCikisDetay] (
    [StokCikisDetayId] INT              IDENTITY (1, 1) NOT NULL,
    [StokCikisId]      INT              NOT NULL,
    [StokGirisDetayId] INT              NOT NULL,
    [SiraNo]           INT              NULL,
    [Miktar]           INT              NULL,
    [InvoiceDetailId]  UNIQUEIDENTIFIER NULL,
    [InvoiceAmount]    DECIMAL (18, 2)  NULL,
    [TpsCikisSiraNo]   INT              NULL,
    [NetKg]            DECIMAL (18, 2)  NULL,
    [BrutKg]           DECIMAL (18, 2)  NULL,
    [BirimTutar]       DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK__ChepStok__7A5C97E41CA2F026] PRIMARY KEY CLUSTERED ([StokCikisDetayId] ASC),
    CONSTRAINT [FK_ChepStokCikisDetay_ChepStokCikis] FOREIGN KEY ([StokCikisId]) REFERENCES [dbo].[ChepStokCikis] ([StokCikisId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChepStokCikisDetay_ChepStokGirisDetay] FOREIGN KEY ([StokGirisDetayId]) REFERENCES [dbo].[ChepStokGirisDetay] ([StokGirisDetayId])
);















