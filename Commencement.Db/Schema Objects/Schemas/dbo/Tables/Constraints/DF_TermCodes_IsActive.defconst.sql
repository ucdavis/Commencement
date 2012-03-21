ALTER TABLE [dbo].[TermCodes]
    ADD CONSTRAINT [DF_TermCodes_IsActive] DEFAULT ((0)) FOR [IsActive];

