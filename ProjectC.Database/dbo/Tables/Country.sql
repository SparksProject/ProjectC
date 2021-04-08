CREATE TABLE [dbo].[Country] (
    [CountryId]   INT           IDENTITY (1, 1) NOT NULL,
    [CountryName] NVARCHAR (50) NOT NULL,
    [EdiCode]     NVARCHAR (3)  NOT NULL,
    [IsoCode]     NVARCHAR (2)  NULL,
    [Status]      BIT           NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

