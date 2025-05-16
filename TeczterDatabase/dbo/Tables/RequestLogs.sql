CREATE TABLE [dbo].[RequestLogs] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp]  DATETIME2 (7)  NOT NULL,
    [User]       NVARCHAR (MAX) NULL,
    [Path]       NVARCHAR (MAX) NULL,
    [Method]     NVARCHAR (MAX) NULL,
    [Query]      NVARCHAR (MAX) NULL,
    [StatusCode] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_RequestLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

