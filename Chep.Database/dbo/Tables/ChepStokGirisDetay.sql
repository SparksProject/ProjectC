﻿CREATE TABLE [dbo].[ChepStokGirisDetay] (
    [StokGirisDetayId] INT           IDENTITY (1, 1) NOT NULL,
    [StokGirisId]      INT           NULL,
    [TpsSiraNo]        INT           NULL,
    [TpsBeyan]         NVARCHAR (20) NULL,
    [EsyaCinsi]        NVARCHAR (20) NULL,
    [EsyaGtip]         NVARCHAR (12) NULL,
    [FaturaNo]         NVARCHAR (30) NULL,
    [FaturaTarih]      DATETIME      NULL,
    [FaturaTutar]      DECIMAL (18)  NULL,
    [FaturaDovizKod]   NVARCHAR (3)  NULL,
    [Miktar]           INT           NULL,
    [OlcuBirimi]       NVARCHAR (5)  NULL,
    [Rejim]            NVARCHAR (4)  NULL,
    [CikisRejimi]      NVARCHAR (4)  NULL,
    [GidecegiUlke]     NVARCHAR (20) NULL,
    [MenseUlke]        NVARCHAR (20) NULL,
    [SozlesmeUlke]     NVARCHAR (20) NULL,
    [Marka]            NVARCHAR (50) NULL,
    [Model]            NVARCHAR (50) NULL,
    [UrunKod]          NVARCHAR (50) NULL,
    [PoNo]             NVARCHAR (50) NULL,
    CONSTRAINT [PK__ChepStok__5F1F65B0F75ADB99] PRIMARY KEY CLUSTERED ([StokGirisDetayId] ASC),
    CONSTRAINT [FK_ChepStokGirisDetay_ChepStokGiris] FOREIGN KEY ([StokGirisId]) REFERENCES [dbo].[ChepStokGiris] ([StokGirisId])
);


