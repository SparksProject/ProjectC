CREATE TABLE [dbo].[MailReport] (
    [MailReportId]   INT            IDENTITY (1, 1) NOT NULL,
    [MailReportName] NVARCHAR (500) NOT NULL,
    [Subject]        NVARCHAR (500) NOT NULL,
    [BodyTemplate]   NVARCHAR (MAX) NOT NULL,
    [SqlScript]      NVARCHAR (MAX) NOT NULL,
    [PeriodTypeId]   INT            NOT NULL,
    [PeriodValue]    NVARCHAR (100) NULL,
    [RecordStatusId] TINYINT        NOT NULL,
    [CreatedDate]    DATETIME       NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [ModifiedDate]   DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    [DeletedDate]    DATETIME       NULL,
    [DeletedBy]      INT            NULL,
    [PeriodDay]      NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_MailReport] PRIMARY KEY CLUSTERED ([MailReportId] ASC),
    CONSTRAINT [FK_MailReport_PeriodType] FOREIGN KEY ([PeriodTypeId]) REFERENCES [dbo].[PeriodType] ([PeriodTypeId]),
    CONSTRAINT [FK_MailReport_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId])
);

