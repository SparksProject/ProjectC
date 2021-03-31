CREATE TABLE [dbo].[WorkOrderLog] (
    [WorkOrderLogId] UNIQUEIDENTIFIER CONSTRAINT [DF_WorkOrderLog_WorkOrderLogId] DEFAULT (newid()) NOT NULL,
    [WorkOrderNo]    NVARCHAR (30)    NOT NULL,
    [Error]          VARCHAR (MAX)    NOT NULL,
    [InsDate]        DATETIME         CONSTRAINT [DF_WorkOrderLog_InsDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WorkOrderLog] PRIMARY KEY CLUSTERED ([WorkOrderLogId] ASC)
);

