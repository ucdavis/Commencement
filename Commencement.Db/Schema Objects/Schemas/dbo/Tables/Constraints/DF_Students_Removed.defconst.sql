ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [DF_Students_Removed] DEFAULT ((0)) FOR [Blocked];

