CREATE TABLE [dbo].[Customer] (
    [CustomerId]     UNIQUEIDENTIFIER CONSTRAINT [DF_Customer_CustomerId] DEFAULT (newid()) NOT NULL,
    [Name]           NVARCHAR (100)   NOT NULL,
    [TaxNo]          NVARCHAR (20)    NOT NULL,
    [TaxName]        NVARCHAR (100)   NOT NULL,
    [Adress]         NVARCHAR (500)   NOT NULL,
    [City]           NVARCHAR (100)   NOT NULL,
    [Country]        NVARCHAR (100)   NOT NULL,
    [Telephone]      NVARCHAR (100)   NULL,
    [OtherId]        NVARCHAR (20)    NULL,
    [UserNameWs]     NVARCHAR (20)    NULL,
    [PasswordWs]     NVARCHAR (20)    NULL,
    [RecordStatusId] TINYINT          CONSTRAINT [DF_Customer_RecordStatusId] DEFAULT ((1)) NOT NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_Customer_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT              NULL,
    [ModifiedDate]   DATETIME         NULL,
    [ModifiedBy]     INT              NULL,
    [DeletedDate]    DATETIME         NULL,
    [DeletedBy]      INT              NULL,
    [MailPeriodType] INT              NULL,
    [MailTime]       INT              NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [FK_Customer_RecordStatuses] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_Customer_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Customer_Users1] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Customer_Users2] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[User] ([UserId])
);



