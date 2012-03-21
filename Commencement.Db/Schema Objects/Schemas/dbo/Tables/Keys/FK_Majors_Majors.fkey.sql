ALTER TABLE [dbo].[Majors]
    ADD CONSTRAINT [FK_Majors_Majors] FOREIGN KEY ([ConsolidationCode]) REFERENCES [dbo].[Majors] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

