CREATE TABLE [dbo].[Product] (
    [ProductId]       UNIQUEIDENTIFIER NOT NULL,
    [CustomerId]      UNIQUEIDENTIFIER NOT NULL,
    [ProductNo]       NVARCHAR (50)    NOT NULL,
    [ProductNameTr]   NVARCHAR (100)   NOT NULL,
    [ProductNameEng]  NVARCHAR (100)   NULL,
    [ProductNameOrg]  NVARCHAR (100)   NULL,
    [HsCode]          NVARCHAR (16)    NOT NULL,
    [Uom]             NCHAR (3)        NULL,
    [GrossWeight]     FLOAT (53)       NULL,
    [NetWeight]       FLOAT (53)       NULL,
    [SapCode]         NVARCHAR (50)    NULL,
    [CountryOfOrigin] NVARCHAR (3)     NULL,
    [RecordStatusId]  TINYINT          NOT NULL,
    [CreatedDate]     DATETIME         NOT NULL,
    [CreatedBy]       INT              NULL,
    [ModifiedDate]    DATETIME         NULL,
    [ModifiedBy]      INT              NULL,
    [DeletedDate]     DATETIME         NULL,
    [DeletedBy]       INT              NULL,
    [UnitPrice]       DECIMAL (18, 2)  NULL,
    [CurrencyType]    NVARCHAR (3)     NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC),
    CONSTRAINT [FK_Product_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_Product_RecordStatuses] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_Product_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Product_Users1] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_Product_Users2] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[User] ([UserId])
);





