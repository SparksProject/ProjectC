﻿CREATE TABLE [dbo].[UserPermission] (
    [UserPermissionId]     INT      IDENTITY (1, 1) NOT NULL,
    [UserId]               INT      NOT NULL,
    [CompanyEdit]          BIT      NOT NULL,
    [CustomerList]         BIT      NOT NULL,
    [CustomerAdd]          BIT      NOT NULL,
    [CustomerGet]          BIT      NOT NULL,
    [CustomerEdit]         BIT      NOT NULL,
    [GenericReportList]    BIT      NOT NULL,
    [GenericReportAdd]     BIT      NOT NULL,
    [GenericReportGet]     BIT      NOT NULL,
    [GenericReportEdit]    BIT      NOT NULL,
    [GenericReportExecute] BIT      NOT NULL,
    [MailDefinitionList]   BIT      NOT NULL,
    [MailDefinitionAdd]    BIT      NOT NULL,
    [MailDefinitionGert]   BIT      NOT NULL,
    [MailDefinitionEdit]   BIT      NOT NULL,
    [ProductList]          BIT      NOT NULL,
    [ProductAdd]           BIT      NOT NULL,
    [ProductGet]           BIT      NOT NULL,
    [ProductEdit]          BIT      NOT NULL,
    [MailReportList]       BIT      NOT NULL,
    [MailReportAdd]        BIT      NOT NULL,
    [MailReportGet]        BIT      NOT NULL,
    [MailReportEdit]       BIT      NOT NULL,
    [MailReportExecute]    BIT      NOT NULL,
    [WorkOrderMasterList]  BIT      NOT NULL,
    [WorkOrderMasterAdd]   BIT      NOT NULL,
    [WorkOrderMasterGet]   BIT      NOT NULL,
    [WorkOrderMasterEdit]  BIT      NOT NULL,
    [UserList]             BIT      NOT NULL,
    [UserAdd]              BIT      NOT NULL,
    [UserGet]              BIT      NOT NULL,
    [UserEdit]             BIT      NOT NULL,
    [CreatedDate]          DATETIME NOT NULL,
    [ModifiedDate]         DATETIME NULL,
    [SparksArchiveImport]  BIT      CONSTRAINT [DF_UserPermission_SparksArchiveImport] DEFAULT ((0)) NOT NULL,
    [SparksArchiveList]    BIT      CONSTRAINT [DF_UserPermission_SparksArchiveList] DEFAULT ((0)) NOT NULL,
    [EvrimArchiveList]     BIT      CONSTRAINT [DF_UserPermission_SparksArchiveList1] DEFAULT ((0)) NOT NULL,
    [BeyannameList]        BIT      CONSTRAINT [DF_UserPermission_BeyannameList] DEFAULT ((0)) NOT NULL,
    [StokGirisList]        BIT      NOT NULL,
    [StokGirisEdit]        BIT      NOT NULL,
    [StokGirisGet]         BIT      NOT NULL,
    [StokGirisAdd]         BIT      NOT NULL,
    [StokCikisList]        BIT      NOT NULL,
    [StokCikisEdit]        BIT      NOT NULL,
    [StokCikisAdd]         BIT      NOT NULL,
    [StokCikisGet]         BIT      NOT NULL,
    CONSTRAINT [PK__UserPerm__A90F88B23AC71F0C] PRIMARY KEY CLUSTERED ([UserPermissionId] ASC),
    CONSTRAINT [FK_UserPermission_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

