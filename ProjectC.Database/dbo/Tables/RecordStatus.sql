CREATE TABLE [dbo].[RecordStatus] (
    [RecordStatusId]   TINYINT       IDENTITY (1, 1) NOT NULL,
    [RecordStatusName] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_RecordStatuses] PRIMARY KEY CLUSTERED ([RecordStatusId] ASC)
);

