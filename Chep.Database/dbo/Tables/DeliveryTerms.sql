CREATE TABLE [dbo].[DeliveryTerms] (
    [DeliveryTermsId] INT           IDENTITY (1, 1) NOT NULL,
    [EdiCode]         VARCHAR (3)   NULL,
    [Name]            VARCHAR (100) NULL,
    CONSTRAINT [PK_DeliveryTerms] PRIMARY KEY CLUSTERED ([DeliveryTermsId] ASC)
);

