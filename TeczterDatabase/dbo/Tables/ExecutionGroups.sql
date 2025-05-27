CREATE TABLE [dbo].[ExecutionGroups] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [CreatedOn]             DATETIME2 (7)  NOT NULL,
    [CreatedById]           INT            NOT NULL,
    [RevisedOn]             DATETIME2 (7)  NOT NULL,
    [RevisedById]           INT            NOT NULL,
    [IsDeleted]             BIT            NOT NULL,
    [ExecutionGroupName]    NVARCHAR (450) NOT NULL,
    [ClosedDate]            DATETIME2 (7)  NULL,
    [ExecutionGroupNotes]   NVARCHAR (MAX) NOT NULL,
    [SoftwareVersionNumber] NVARCHAR (MAX) NULL,
    [RowVersion]            ROWVERSION     NOT NULL,
    CONSTRAINT [PK_ExecutionGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExecutionGroups_ExecutionGroupName]
    ON [dbo].[ExecutionGroups]([ExecutionGroupName] ASC);

