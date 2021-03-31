﻿CREATE TABLE [dbo].[InvoiceDetail] (
    [InvoiceDetailId]   UNIQUEIDENTIFIER CONSTRAINT [DF_InvoiceDetails_InvoiceDetailId] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [InvoiceId]         UNIQUEIDENTIFIER NOT NULL,
    [HsCode]            NVARCHAR (16)    NULL,
    [DescGoods]         NVARCHAR (250)   NOT NULL,
    [ProductNo]         NVARCHAR (50)    NOT NULL,
    [CountryOfOrigin]   NVARCHAR (50)    NULL,
    [Uom]               NVARCHAR (3)     NOT NULL,
    [ActualQuantity]    FLOAT (53)       NOT NULL,
    [InvoiceQuantity]   FLOAT (53)       NULL,
    [GrossWeight]       FLOAT (53)       NULL,
    [NetWeight]         FLOAT (53)       NOT NULL,
    [IntrnlAgmt]        NCHAR (10)       NULL,
    [InvoiceNo]         NVARCHAR (50)    NOT NULL,
    [InvoiceDate]       DATE             NOT NULL,
    [InvoiceAmount]     FLOAT (53)       NOT NULL,
    [PkgType]           NCHAR (10)       NOT NULL,
    [CommclDesc]        NVARCHAR (240)   NULL,
    [NumberOfPackages]  INT              NOT NULL,
    [RecordStatusId]    TINYINT          CONSTRAINT [DF_InvoiceDetail_RecordStatusId] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_InvoiceDetail_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT              NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        INT              NULL,
    [DeletedDate]       DATETIME         NULL,
    [DeletedBy]         INT              NULL,
    [FileNumber]        VARCHAR (50)     NULL,
    [ItemNumber]        INT              NOT NULL,
    [ProducerCompanyNo] NVARCHAR (30)    NULL,
    [ProducerCompany]   NVARCHAR (250)   NULL,
    [IncentiveLineNo]   NVARCHAR (250)   NULL,
    CONSTRAINT [PK_InvoiceDetails] PRIMARY KEY CLUSTERED ([InvoiceDetailId] ASC),
    CONSTRAINT [FK_InvoiceDetail_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([InvoiceId]),
    CONSTRAINT [FK_InvoiceDetails_Invoices] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[Invoice] ([InvoiceId]) ON DELETE CASCADE
);

