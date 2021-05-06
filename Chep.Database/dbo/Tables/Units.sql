CREATE TABLE [dbo].[Units] (
    [UnitsId]   UNIQUEIDENTIFIER CONSTRAINT [DF_Units_UnitsId] DEFAULT (newid()) NOT NULL,
    [EdiCode]   VARCHAR (3)      NOT NULL,
    [UnitsName] VARCHAR (100)    NOT NULL,
    [Status]    BIT              CONSTRAINT [DF_Units_Status] DEFAULT ((1)) NOT NULL
);

 

