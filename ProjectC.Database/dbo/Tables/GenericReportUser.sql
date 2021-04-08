CREATE TABLE [dbo].[GenericReportUser] (
    [GenericReportUserId] INT IDENTITY (1, 1) NOT NULL,
    [GenericReportId]     INT NOT NULL,
    [UserId]              INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GenericReportUserId] ASC),
    CONSTRAINT [FK_GenericReportUser_GenericReport] FOREIGN KEY ([GenericReportId]) REFERENCES [dbo].[GenericReport] ([GenericReportId]),
    CONSTRAINT [FK_GenericReportUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);



