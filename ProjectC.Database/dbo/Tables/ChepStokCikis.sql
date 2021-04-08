CREATE TABLE [dbo].[ChepStokCikis] (
    [StokCikisId]     INT              IDENTITY (1, 1) NOT NULL,
    [ReferansNo]      NVARCHAR (20)    NULL,
    [IslemTarihi]     DATETIME         NULL,
    [BeyannameNo]     NVARCHAR (20)    NULL,
    [BeyannameTarihi] DATETIME         NULL,
    [IhracatciFirma]  UNIQUEIDENTIFIER NULL,
    [TpsNo]           NVARCHAR (50)    NULL,
    [TpsTarih]        DATETIME         NULL,
    CONSTRAINT [PK__ChepStok__FDAB99CF950106B1] PRIMARY KEY CLUSTERED ([StokCikisId] ASC),
    CONSTRAINT [FK_ChepStokCikis_Customer] FOREIGN KEY ([IhracatciFirma]) REFERENCES [dbo].[Customer] ([CustomerId])
);



