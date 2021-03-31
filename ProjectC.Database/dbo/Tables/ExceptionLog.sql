CREATE TABLE [dbo].[ExceptionLog] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [SpecialMessage] NVARCHAR (MAX) NULL,
    [InnerException] NVARCHAR (MAX) NULL,
    [StackTrace]     NVARCHAR (MAX) NULL,
    [ExceptionDate]  DATETIME       NOT NULL,
    CONSTRAINT [PK_ExceptionLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

