CREATE TABLE [dbo].[UserCustomer] (
    [UserCustomerId] INT              IDENTITY (1, 1) NOT NULL,
    [UserId]         INT              NOT NULL,
    [CustomerId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserCustomer] PRIMARY KEY CLUSTERED ([UserCustomerId] ASC)
);

