CREATE TABLE [dbo].[PeriodType] (
    [PeriodTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [PeriodTypeName] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_PeriodTypes] PRIMARY KEY CLUSTERED ([PeriodTypeId] ASC)
);

