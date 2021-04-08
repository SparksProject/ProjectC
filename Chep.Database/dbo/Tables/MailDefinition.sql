CREATE TABLE [dbo].[MailDefinition] (
    [MailDefinitionId] INT            IDENTITY (1, 1) NOT NULL,
    [RecipientName]    NVARCHAR (100) NOT NULL,
    [EmailAddress]     NVARCHAR (100) NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    CONSTRAINT [PK_MailDefinition] PRIMARY KEY CLUSTERED ([MailDefinitionId] ASC)
);

