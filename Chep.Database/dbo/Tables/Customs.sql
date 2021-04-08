CREATE TABLE [dbo].[Customs] (
    [CustomsId]   INT            IDENTITY (1, 1) NOT NULL,
    [EdiCode]     NVARCHAR (6)   NOT NULL,
    [CustomsName] NVARCHAR (250) NOT NULL,
    [Status]      BIT            NOT NULL,
    CONSTRAINT [PK_Customs] PRIMARY KEY CLUSTERED ([CustomsId] ASC)
);

