CREATE TABLE [dbo].[Units](
	[UnitsId] [uniqueidentifier] NOT NULL,
	[EdiCode] [varchar](3) NOT NULL,
	[UnitsName] [varchar](100) NOT NULL,
	[Status] [bit] NOT NULL default 0,
	CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED ([UnitsId] ASC)
) 

