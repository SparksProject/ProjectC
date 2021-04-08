CREATE TABLE [dbo].[MailReportUser] (
    [MailReportUserId] INT     IDENTITY (1, 1) NOT NULL,
    [MailReportId]     INT     NOT NULL,
    [MailDefinitionId] INT     NOT NULL,
    [ReceiverTypeId]   TINYINT NOT NULL,
    CONSTRAINT [PK_MailReportUser] PRIMARY KEY CLUSTERED ([MailReportUserId] ASC),
    CONSTRAINT [FK_MailReportUser_MailDefinition] FOREIGN KEY ([MailDefinitionId]) REFERENCES [dbo].[MailDefinition] ([MailDefinitionId]),
    CONSTRAINT [FK_MailReportUser_MailReport] FOREIGN KEY ([MailReportId]) REFERENCES [dbo].[MailReport] ([MailReportId])
);

