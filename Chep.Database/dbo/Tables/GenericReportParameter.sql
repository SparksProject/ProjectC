CREATE TABLE [dbo].[GenericReportParameter] (
    [GenericReportParameterId]   INT            IDENTITY (1, 1) NOT NULL,
    [GenericReportId]            INT            NOT NULL,
    [GenericReportParameterName] NVARCHAR (100) NOT NULL,
    [ParameterLabel]             NVARCHAR (100) NOT NULL,
    [ParameterType]              SMALLINT       NOT NULL,
    CONSTRAINT [PK_GenericReportParameter] PRIMARY KEY CLUSTERED ([GenericReportParameterId] ASC),
    CONSTRAINT [FK_GenericReportParameter_GenericReport] FOREIGN KEY ([GenericReportId]) REFERENCES [dbo].[GenericReport] ([GenericReportId])
);

