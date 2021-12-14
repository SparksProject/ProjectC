CREATE TABLE [dbo].[ChepStokCikis] (
    [StokCikisId]      INT              IDENTITY (1, 1) NOT NULL,
    [ReferansNo]       INT              NOT NULL,
    [IslemTarihi]      DATETIME         NULL,
    [BeyannameNo]      NVARCHAR (20)    NULL,
    [BeyannameTarihi]  DATETIME         NULL,
    [IhracatciFirma]   UNIQUEIDENTIFIER NULL,
    [TpsNo]            NVARCHAR (50)    NULL,
    [TpsTarih]         DATETIME         NULL,
    [InvoiceId]        UNIQUEIDENTIFIER NULL,
    [InvoiceNo]        NVARCHAR (50)    NULL,
    [InvoiceDate]      DATE             NULL,
    [InvoiceAmount]    DECIMAL (18, 2)  NULL,
    [InvoiceCurrency]  NVARCHAR (3)     NULL,
    [GtbReferenceNo]   NVARCHAR (25)    NULL,
    [AliciFirma]       UNIQUEIDENTIFIER NULL,
    [TeslimSekli]      NVARCHAR (3)     NULL,
    [CikisGumruk]      NVARCHAR (6)     NULL,
    [OdemeSekli]       NVARCHAR (2)     NULL,
    [NakliyeciFirma]   UNIQUEIDENTIFIER NULL,
    [CikisAracKimligi] NVARCHAR (35)    NULL,
    [KapCinsi]         NVARCHAR (2)     NULL,
    [KapMiktari]       INT              NULL,
    [IsEmriDurum]      NVARCHAR (300)   NULL,
    [SiparisNo]        NVARCHAR (50)    NULL,
    CONSTRAINT [PK__ChepStok__FDAB99CF950106B1] PRIMARY KEY CLUSTERED ([StokCikisId] ASC),
    CONSTRAINT [FK_ChepStokCikis_Customer] FOREIGN KEY ([IhracatciFirma]) REFERENCES [dbo].[Customer] ([CustomerId])
);















