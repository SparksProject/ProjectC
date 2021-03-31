CREATE TABLE [dbo].[ChepStokGiris] (
    [StokGirisId]     INT            IDENTITY (1, 1) NOT NULL,
    [ReferansNo]      NVARCHAR (16)  NOT NULL,
    [TPSNo]           NVARCHAR (30)  NOT NULL,
    [TPSDurum]        NVARCHAR (50)  NOT NULL,
    [BasvuruTarihi]   DATETIME       NULL,
    [SureSonuTarihi]  DATETIME       NULL,
    [GumrukKod]       NVARCHAR (6)   NULL,
    [BeyannameNo]     NVARCHAR (16)  NULL,
    [BeyannameTarihi] DATETIME       NULL,
    [BelgeAd]         NVARCHAR (50)  NULL,
    [BelgeSart]       NVARCHAR (50)  NULL,
    [TPSAciklama]     NVARCHAR (250) NULL,
    [IthalatciFirma]  INT            NULL,
    [IhracatciFirma]  INT            NULL,
    [KapAdet]         INT            NULL,
    CONSTRAINT [PK__ChepStok__41A2AA5533234034] PRIMARY KEY CLUSTERED ([StokGirisId] ASC)
);

