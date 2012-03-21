CREATE TABLE [dbo].[GeVersions] (
    [Version]                INT            NOT NULL,
    [GeSubjectAreaCode]      CHAR (1)       NOT NULL,
    [MajorInd]               BIT            NULL,
    [CourseAttributeInd]     BIT            NULL,
    [GeSubjectAreaComponent] CHAR (1)       NULL,
    [Credits]                DECIMAL (5, 3) NULL,
    [SortOrder]              INT            NULL
);

