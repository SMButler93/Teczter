CREATE TABLE [dbo].[ErrorLogs] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [TimeStamp]             DATETIME2 (7)  NOT NULL,
    [User]                  NVARCHAR (MAX) NULL,
    [ExceptionType]         NVARCHAR (MAX) NOT NULL,
    [StackTrace]            NVARCHAR (MAX) NULL,
    [Message]               NVARCHAR (MAX) NULL,
    [InnerExceptionMessage] NVARCHAR (MAX) NULL,
    [RequestLogId]          INT            NULL,
    CONSTRAINT [PK_ErrorLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ErrorLogs_RequestLogs_RequestLogId] FOREIGN KEY ([RequestLogId]) REFERENCES [dbo].[RequestLogs] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ErrorLogs_RequestLogId]
    ON [dbo].[ErrorLogs]([RequestLogId] ASC) WHERE ([RequestLogId] IS NOT NULL);

