CREATE TABLE [dbo].[WorkOrderSend] (
    [WorkOrderSendId]   INT              IDENTITY (1, 1) NOT NULL,
    [INSDATE]           DATETIME         CONSTRAINT [DF_WorkOrderSend_InsDate] DEFAULT (getdate()) NOT NULL,
    [XMLTEXT]           NVARCHAR (MAX)   NULL,
    [EXPNO]             NVARCHAR (16)    NULL,
    [STATU]             NVARCHAR (25)    NULL,
    [DOSYANO]           VARCHAR (20)     NULL,
    [PROCESSSTATUS]     TINYINT          CONSTRAINT [DF_WorkOrderSend_ProcessStatus] DEFAULT ((0)) NOT NULL,
    [UPDDATE]           DATETIME         NULL,
    [WorkOrderNo]       NVARCHAR (30)    NULL,
    [WorkOrderMasterId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_WorkOrderSend] PRIMARY KEY CLUSTERED ([WorkOrderSendId] ASC)
);

