CREATE TABLE [dbo].[SparksArchive] (
    [ArchiveId]  UNIQUEIDENTIFIER CONSTRAINT [DF_Archive_ArchiveId] DEFAULT (newid()) NOT NULL,
    [DosyaTipi]  VARCHAR (250)    NULL,
    [DosyaNo]    VARCHAR (250)    NULL,
    [CustomerId] UNIQUEIDENTIFIER NOT NULL,
    [BelgeAdi]   VARCHAR (250)    NULL,
    [DosyaYolu]  VARCHAR (500)    NULL,
    [Gosterme]   BIT              NOT NULL,
    [InsDate]    DATETIME         CONSTRAINT [DF_SparksArchive_Insdate] DEFAULT (getdate()) NOT NULL,
    [FileDate]   VARCHAR (50)     NULL,
    CONSTRAINT [PK_SparksArchive] PRIMARY KEY CLUSTERED ([ArchiveId] ASC),
    CONSTRAINT [FK_SparksArchive_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);



