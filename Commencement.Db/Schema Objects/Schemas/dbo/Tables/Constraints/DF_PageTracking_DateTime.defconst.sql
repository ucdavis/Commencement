ALTER TABLE [dbo].[PageTracking]
    ADD CONSTRAINT [DF_PageTracking_DateTime] DEFAULT (getdate()) FOR [DateTime];

