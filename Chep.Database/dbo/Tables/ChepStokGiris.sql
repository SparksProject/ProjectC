CREATE TABLE [dbo].[ChepStokGiris] (
    [StokGirisId]     INT              IDENTITY (1, 1) NOT NULL,
    [ReferansNo]      NVARCHAR (16)    NOT NULL,
    [TpsNo]           NVARCHAR (30)    NOT NULL,
    [TpsDurum]        NVARCHAR (50)    NOT NULL,
    [BasvuruTarihi]   DATETIME         NULL,
    [SureSonuTarihi]  DATETIME         NULL,
    [GumrukKod]       NVARCHAR (6)     NULL,
    [BeyannameNo]     NVARCHAR (16)    NULL,
    [BeyannameTarihi] DATETIME         NULL,
    [BelgeAd]         NVARCHAR (50)    NULL,
    [BelgeSart]       NVARCHAR (50)    NULL,
    [TpsAciklama]     NVARCHAR (250)   NULL,
    [IthalatciFirma]  UNIQUEIDENTIFIER NULL,
    [IhracatciFirma]  UNIQUEIDENTIFIER NULL,
    [KapAdet]         INT              NULL,
    CONSTRAINT [PK__ChepStok__41A2AA5533234034] PRIMARY KEY CLUSTERED ([StokGirisId] ASC),
    CONSTRAINT [FK_ChepStokGiris_Customer] FOREIGN KEY ([IthalatciFirma]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_ChepStokGiris_Customer1] FOREIGN KEY ([IhracatciFirma]) REFERENCES [dbo].[Customer] ([CustomerId])
);



