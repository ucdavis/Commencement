﻿ALTER TABLE [dbo].[StudentMajors]
    ADD CONSTRAINT [PK_StudentMajors] PRIMARY KEY CLUSTERED ([Student_Id] ASC, [MajorCode] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

