ALTER TABLE [dbo].[PageTracking]
    ADD CONSTRAINT [DF_PageTracking_IsEmulating] DEFAULT ((0)) FOR [IsEmulating];

