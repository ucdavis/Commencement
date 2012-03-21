ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [DF_Students_DateUpdated] DEFAULT (getdate()) FOR [DateUpdated];

