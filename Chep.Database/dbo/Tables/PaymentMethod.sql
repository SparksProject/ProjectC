CREATE TABLE [dbo].[PaymentMethod] (
    [PaymentMethodId] INT           IDENTITY (1, 1) NOT NULL,
    [EdiCode]         VARCHAR (3)   NULL,
    [Name]            VARCHAR (100) NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC)
);

