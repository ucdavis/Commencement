﻿ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([PIDM] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

