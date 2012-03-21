ALTER TABLE [dbo].[Majors]
    ADD CONSTRAINT [DF_Majors_IsActive] DEFAULT ((1)) FOR [IsActive];

