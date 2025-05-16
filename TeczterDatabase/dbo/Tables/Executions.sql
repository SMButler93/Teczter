CREATE TABLE [dbo].[Executions] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedById]      INT              NOT NULL,
    [RevisedOn]        DATETIME2 (7)    NOT NULL,
    [RevisedById]      INT              NOT NULL,
    [IsDeleted]        BIT              NOT NULL,
    [ExecutionGroupId] INT              NOT NULL,
    [AssignedUserId]   UNIQUEIDENTIFIER NULL,
    [TestId]           INT              NOT NULL,
    [FailedStepId]     INT              NULL,
    [FailureReason]    NVARCHAR (250)   NULL,
    [TestedById]       INT              NULL,
    [Notes]            NVARCHAR (MAX)   DEFAULT (N'[]') NOT NULL,
    [ExecutionState]   INT              NOT NULL,
    CONSTRAINT [PK_Executions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Executions_ExecutionGroups_ExecutionGroupId] FOREIGN KEY ([ExecutionGroupId]) REFERENCES [dbo].[ExecutionGroups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Executions_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [dbo].[Tests] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Executions_TestSteps_FailedStepId] FOREIGN KEY ([FailedStepId]) REFERENCES [dbo].[TestSteps] ([Id]),
    CONSTRAINT [FK_Executions_Users_AssignedUserId] FOREIGN KEY ([AssignedUserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Executions_AssignedUserId]
    ON [dbo].[Executions]([AssignedUserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Executions_ExecutionGroupId]
    ON [dbo].[Executions]([ExecutionGroupId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Executions_FailedStepId]
    ON [dbo].[Executions]([FailedStepId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Executions_TestId]
    ON [dbo].[Executions]([TestId] ASC);

