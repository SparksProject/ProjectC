CREATE TABLE [dbo].[WorkOrderMaster] (
    [WorkOrderMasterId] UNIQUEIDENTIFIER NOT NULL,
    [WorkOrderNo]       NVARCHAR (30)    NOT NULL,
    [DeclarationType]   NCHAR (2)        NOT NULL,
    [Status]            INT              CONSTRAINT [DF_WorkOrderMaster_Status] DEFAULT ((20)) NOT NULL,
    [MasterId]          UNIQUEIDENTIFIER NULL,
    [CustomerId]        UNIQUEIDENTIFIER NOT NULL,
    [Message]           NTEXT            NULL,
    [RecordStatusId]    TINYINT          CONSTRAINT [DF_WorkOrderMaster_RecordStatusId] DEFAULT ((1)) NOT NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_WorkOrderMaster_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT              NULL,
    [ModifiedDate]      DATETIME         NULL,
    [ModifiedBy]        INT              NULL,
    [DeletedDate]       DATETIME         NULL,
    [DeletedBy]         INT              NULL,
    [DosyaNo]           NVARCHAR (20)    NULL,
    CONSTRAINT [PK_WorkOrderMasters] PRIMARY KEY CLUSTERED ([WorkOrderMasterId] ASC),
    CONSTRAINT [FK_WorkOrderMaster_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_WorkOrderMaster_RecordStatuses] FOREIGN KEY ([RecordStatusId]) REFERENCES [dbo].[RecordStatus] ([RecordStatusId]),
    CONSTRAINT [FK_WorkOrderMaster_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_WorkOrderMaster_Users1] FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_WorkOrderMaster_Users2] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[User] ([UserId])
);

