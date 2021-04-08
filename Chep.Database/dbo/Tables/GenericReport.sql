CREATE TABLE [dbo].[GenericReport] (
    [GenericReportId]   INT            IDENTITY (1, 1) NOT NULL,
    [GenericReportName] NVARCHAR (500) NOT NULL,
    [SqlScript]         NVARCHAR (MAX) NOT NULL,
    [IsDefaultReport]   BIT            CONSTRAINT [DF_GenericReport_IsDefaultReport] DEFAULT ((0)) NOT NULL,
    [RecordStatusId]    TINYINT        NOT NULL,
    [CreatedDate]       DATETIME       NOT NULL,
    [CreatedBy]         INT            NOT NULL,
    [ModifiedDate]      DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [DeletedDate]       DATETIME       NULL,
    [DeletedBy]         INT            NULL,
    CONSTRAINT [PK_GenericReport] PRIMARY KEY CLUSTERED ([GenericReportId] ASC),
    CONSTRAINT [FK_GenericReport_RecordStatus] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId])
);

