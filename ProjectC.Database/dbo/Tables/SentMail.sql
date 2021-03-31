CREATE TABLE [dbo].[SentMail] (
    [SentMailId]   INT             IDENTITY (1, 1) NOT NULL,
    [MailReportId] INT             NOT NULL,
    [Subject]      NVARCHAR (500)  NOT NULL,
    [Body]         NVARCHAR (2000) NOT NULL,
    [EmailAddress] NVARCHAR (150)  NOT NULL,
    [SentDate]     DATETIME        NOT NULL,
    [CreatedDate]  DATETIME        DEFAULT (getdate()) NULL,
    [CreatedBy]    INT             NULL,
    [EmailCc]      VARCHAR (500)   NULL,
    CONSTRAINT [PK_SentMail] PRIMARY KEY CLUSTERED ([SentMailId] ASC),
    CONSTRAINT [FK_SentMail_MailReport] FOREIGN KEY ([MailReportId]) REFERENCES [dbo].[MailReport] ([MailReportId])
);

