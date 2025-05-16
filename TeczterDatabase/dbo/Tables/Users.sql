CREATE TABLE [dbo].[Users] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted]   BIT              NOT NULL,
    [Username]    NVARCHAR (450)   NOT NULL,
    [Email]       NVARCHAR (50)    NOT NULL,
    [Password]    NVARCHAR (25)    NOT NULL,
    [Department]  INT              NOT NULL,
    [AccessLevel] INT              NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]
    ON [dbo].[Users]([Email] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username]
    ON [dbo].[Users]([Username] ASC);

