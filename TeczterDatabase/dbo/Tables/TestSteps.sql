CREATE TABLE [dbo].[TestSteps] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [CreatedOn]     DATETIME2 (7)  NOT NULL,
    [CreatedById]   INT            NOT NULL,
    [RevisedOn]     DATETIME2 (7)  NOT NULL,
    [RevisedById]   INT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    [TestId]        INT            NOT NULL,
    [StepPlacement] INT            NOT NULL,
    [Instructions]  NVARCHAR (750) NOT NULL,
    [Urls]          NVARCHAR (MAX) DEFAULT (N'[]') NOT NULL,
    [RowVersion] ROWVERSION        NOT NULL,
    CONSTRAINT [PK_TestSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TestSteps_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [dbo].[Tests] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_TestSteps_TestId]
    ON [dbo].[TestSteps]([TestId] ASC);

