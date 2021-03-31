CREATE TABLE [dbo].[ChepStokCikis] (
    [StokCikisId]     INT           IDENTITY (1, 1) NOT NULL,
    [ReferansNo]      NVARCHAR (20) NULL,
    [IslemTarihi]     DATETIME      NULL,
    [BeyannameNo]     NVARCHAR (20) NULL,
    [BeyannameTarihi] DATETIME      NULL,
    [IhracatciFirma]  NVARCHAR (50) NULL,
    [TPSNo]           NVARCHAR (50) NULL,
    [TPSTarih]        DATETIME      NULL,
    CONSTRAINT [PK__ChepStok__FDAB99CF950106B1] PRIMARY KEY CLUSTERED ([StokCikisId] ASC)
);

