CREATE TABLE [dbo].[DatamartStudents] (
    [spriden_pidm]                  NUMERIC (8)     NOT NULL,
    [spriden_id]                    VARCHAR (36)    NOT NULL,
    [spriden_first_name]            VARCHAR (240)   NULL,
    [spriden_mi]                    VARCHAR (240)   NULL,
    [spriden_last_name]             VARCHAR (240)   NOT NULL,
    [earnedunits]                   NUMERIC (11, 3) NOT NULL,
    [currentunits]                  NUMERIC (11, 3) NOT NULL,
    [goremal_email_address]         VARCHAR (512)   NULL,
    [loginid]                       VARCHAR (120)   NULL,
    [shrttrm_astd_code_end_of_term] VARCHAR (8)     NULL,
    [sja]                           BIT             NOT NULL,
    [zgvlcfs_majr_code]             VARCHAR (16)    NULL
);

