ALTER TABLE [dbo].[SpecialNeeds]
    ADD CONSTRAINT [DF_SpecialNeeds_IsActive] DEFAULT ((1)) FOR [IsActive];

