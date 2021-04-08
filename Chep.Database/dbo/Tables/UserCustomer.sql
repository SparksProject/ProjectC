CREATE TABLE [dbo].[UserCustomer] (
    [UserCustomerId] INT              IDENTITY (1, 1) NOT NULL,
    [UserId]         INT              NOT NULL,
    [CustomerId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_UserCustomer] PRIMARY KEY CLUSTERED ([UserCustomerId] ASC),
    CONSTRAINT [FK_UserCustomer_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_UserCustomer_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);



