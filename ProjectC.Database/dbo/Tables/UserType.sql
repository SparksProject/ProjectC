CREATE TABLE [dbo].[UserType] (
    [UserTypeId]   TINYINT       IDENTITY (1, 1) NOT NULL,
    [UserTypeName] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([UserTypeId] ASC)
);

