CREATE TABLE [dbo].[GenericReportUser] (
    [GenericReportUserId] INT IDENTITY (1, 1) NOT NULL,
    [GenericReportId]     INT NOT NULL,
    [UserId]              INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GenericReportUserId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK__GenericRe__Gener__2057CCD0] FOREIGN KEY ([GenericReportId]) REFERENCES [dbo].[GenericReport] ([GenericReportId]),
    CONSTRAINT [FK__GenericRe__Gener__5AEE82B9] FOREIGN KEY ([GenericReportId]) REFERENCES [dbo].[GenericReport] ([GenericReportId]),
    CONSTRAINT [FK__GenericRe__UserI__5535A963] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

