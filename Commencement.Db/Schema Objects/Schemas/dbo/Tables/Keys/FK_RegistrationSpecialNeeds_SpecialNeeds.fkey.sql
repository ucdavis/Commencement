ALTER TABLE [dbo].[RegistrationSpecialNeeds]
    ADD CONSTRAINT [FK_RegistrationSpecialNeeds_SpecialNeeds] FOREIGN KEY ([SpecialNeedId]) REFERENCES [dbo].[SpecialNeeds] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

