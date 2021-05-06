CREATE TABLE [dbo].[CurrencyType] (
    [CurrencyTypeId]   UNIQUEIDENTIFIER CONSTRAINT [DF_CurrencyType_CurrencyTypeId] DEFAULT (newid()) NOT NULL,
    [EdiCode]          VARCHAR (3)      NOT NULL,
    [CurrencyTypeName] VARCHAR (100)    NOT NULL,
    [Status]           BIT              CONSTRAINT [DF_CurrencyType_Status] DEFAULT ((1)) NOT NULL
);

