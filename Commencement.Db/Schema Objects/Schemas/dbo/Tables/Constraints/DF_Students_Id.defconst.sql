ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [DF_Students_Id] DEFAULT (newid()) FOR [Id];

