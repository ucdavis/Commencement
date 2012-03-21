ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [DF_EmailQueue_Immediate] DEFAULT ((0)) FOR [Immediate];

