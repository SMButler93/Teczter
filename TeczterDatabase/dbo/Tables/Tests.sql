CREATE TABLE [dbo].[Tests] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [CreatedOn]        DATETIME2 (7)  NOT NULL,
    [CreatedById]      INT            NOT NULL,
    [RevisedOn]        DATETIME2 (7)  NOT NULL,
    [RevisedById]      INT            NOT NULL,
    [IsDeleted]        BIT            NOT NULL,
    [Title]            NVARCHAR (75)  NOT NULL,
    [Description]      NVARCHAR (750) NOT NULL,
    [OwningDepartment] NVARCHAR (MAX) NOT NULL,
    [Urls]             NVARCHAR (MAX) DEFAULT (N'[]') NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Tests] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Tests_Title]
    ON [dbo].[Tests]([Title] ASC);

