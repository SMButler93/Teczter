CREATE TABLE [dbo].[Logs]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Message] NVARCHAR(MAX) NULL,
    [MessageTemplate] NVARCHAR(MAX) NULL,
    [Level] NVARCHAR(128) NULL,
    [TimeStamp] DATETIMEOFFSET NOT NULL,
    [Exception] NVARCHAR(MAX) NULL,
    [Properties] NVARCHAR(MAX) NULL,
    [LogEvent] NVARCHAR(MAX) NULL,
    [MachineName] NVARCHAR(200) NULL,
    [ThreadId] INT NULL
);