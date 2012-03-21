ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [DF_Students_DatedAdded] DEFAULT (getdate()) FOR [DateAdded];

