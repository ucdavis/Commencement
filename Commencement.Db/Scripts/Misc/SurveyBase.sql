/*
Survey field types, taken from SIMS, run the scirpt on sims to get the values

select 'insert into SurveyFieldTypes (name, hasoptions, filterable) values ('''+ name +''', '+ cast(hasoptions as char(1)) + ', ' + cast(filterable as char(1)) + ')'
from FormFieldTypes
*/

SET IDENTITY_INSERT [dbo].[SurveyFieldTypes] ON
GO

INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(1, 'Text Box', 0, 1, 1, 0)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(2, 'Text Area', 0, 1, 1, 0)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(3, 'Boolean', 0, 0, 1, 1)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(4, 'Boolean/Other', 1, 0, 1, 1)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(5, 'Radio Buttons', 1, 0, 1, 1)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(6, 'Checkbox List', 1, 0, 1, 1)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(7, 'Drop Down', 1, 1, 1, 1)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(8, 'Date', 0, 1, 1, 0)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(9, 'No Answer', 0, 0, 0, 0)
INSERT INTO [dbo].[SurveyFieldTypes]([Id], [Name], [HasOptions], [Filterable], [Answerable], [FixedAnswers]) VALUES(10, 'Group Header', 0, 0, 0, 0)
GO

SET IDENTITY_INSERT [dbo].[SurveyFieldTypes] OFF
GO



/*
Regex validators
*/
insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Required', 'required', '^.+$', '{0} is a required field.')
--insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Email', 'email', '(^((([a-z]|\d|[!#\$%&''\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&''\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$){1}|^$', '{0} is not a valid email.')
--insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Url', 'url', '(^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&''\(\)\*\+,;=]|:|@)|\/|\?)*)?$){1}|^$', '{0} is not a valid url.')
--insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Date', 'date', '(^(((0?[1-9]|1[012])[-\/](0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])[-\/](29|30)|(0?[13578]|1[02])[-\/]31)[-\/](19|[2-9]\d)\d{2}|0?2[-\/]29[-\/]((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$){1}|^$', '{0} is not a valid date.')
--insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Phone Number', 'phoneUS', '(^\(?[\d]{3}\)?[\s-]?[\d]{3}[\s-]?[\d]{4}$){1}|^$', '{0} is not a valid phone number.')
--insert into SurveyFieldValidators (name, class, regex, errormessage) values ('Zip Code', 'zipUS', '^\d{5}(-\d{4})?$', '{0} is not a valid zip code.')

/*
Insert the current CAES exit survewy
*/

SET IDENTITY_INSERT [dbo].[Surveys] ON
GO
INSERT INTO [dbo].[Surveys]([Id], [Name], [Created]) VALUES(1, 'CA&ES Commencement Exit Survey', '20130315 07:53:23.917')
GO
SET IDENTITY_INSERT [dbo].[Surveys] OFF
GO

SET IDENTITY_INSERT [dbo].[SurveyFields] ON
GO

INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(1, 1, 'Were you a tansfer student?', 4, 2, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(2, 1, 'I am graduating with a degree in', 7, 3, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(3, 1, 'My ultimate goal for the future is:', 7, 4, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(4, 1, 'Other', 1, 5, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(5, 1, 'If you have already obtained a job or admission to another school/program please share with us where? and if you will receive a salary what will be your average yearly salary be?', 1, 6, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(6, 1, 'I have earned a scholarship(s) while at UC Davis.  If yes, please list name of scholarship(s).', 2, 7, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(7, 1, 'Advising', 10, 8, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(8, 1, 'Student Peer advisor', 5, 9, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(9, 1, 'If yes, I was satisfied with the quality of the advising', 5, 10, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(10, 1, 'Staff advisor', 5, 11, 0, 1, 1)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(11, 1, 'Name (Optional)', 1, 1, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(13, 1, 'If yes, I was satisfied with the quality of the advising', 5, 12, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(15, 1, 'Faculty advisor', 5, 13, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(16, 1, 'If yes, I was satisfied with the quality of the advising', 5, 14, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(17, 1, 'Dean''s Office Staff Advisor', 5, 15, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(19, 1, 'If yes, I was satisfied with the quality of the advising', 5, 16, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(20, 1, 'What advising services did you find most useful?', 2, 17, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(21, 1, 'I know how to obtain advising when I needed it', 5, 18, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(22, 1, 'To what extent did your College of Agricultural & Environmental Sciences EXPERIENCE meet your expectations', 5, 19, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(23, 1, 'I was very satisfied with my major.', 5, 20, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(25, 1, '(if you wish to provide comment)', 2, 21, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(26, 1, 'My overall experience at the College of Agricultural & Environmental Sciences as it relates to divsersity was positive:', 5, 22, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(27, 1, '(if you wish to provide comment)', 2, 23, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(28, 1, 'Extra Curricular Activities', 10, 24, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(29, 1, 'While attending UC Davis, were you involved in any community service projects?  Indicate the number of quarters in which you were involved.  (Enter 0 if none)', 1, 25, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(30, 1, 'While attending UC Davis, were you involved in any internships? (Enter 0 if none)', 1, 26, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(31, 1, 'Did you participate in the Career Discovery Group (CDG) program?', 3, 27, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(32, 1, 'Were you ever a member of FFA?', 3, 28, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(33, 1, 'Were you ever a member of 4H?', 3, 29, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(34, 1, 'Were you an Aggie Ambassador while at UC Davis?', 3, 30, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(36, 1, 'What tools, programs, or events would you be most likely to use to stay connected to the UC Davis College of Agricultural & Environmental Sciences (CA&ES) after you graduate?', 6, 31, 0, 0, 0)
INSERT INTO [dbo].[SurveyFields]([Id], [SurveyId], [Prompt], [SurveyFieldTypeId], [Order], [Hidden], [ShowInTable], [Export]) VALUES(37, 1, 'Did you find the college website useful: caes.ucdavis.edu?', 5, 32, 0, 0, 0)
GO

SET IDENTITY_INSERT [dbo].[SurveyFields] OFF
GO

SET IDENTITY_INSERT [dbo].[SurveyFieldOptions] ON
GO

INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(1, 'If yes, please list which college you transferred from', 1)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(2, 'Agricultural Management & Rangeland Resources', 2)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(3, 'Job / Career in Agriculture (no further education)', 3)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(4, 'Yes', 8)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(5, 'No', 8)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(6, 'Didn''t know there were student advisors', 8)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(7, 'agree strongly', 9)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(8, 'agree', 9)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(9, 'disagree', 9)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(10, 'disagree strongly', 9)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(11, 'neutral', 9)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(12, 'Yes', 10)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(13, 'No', 10)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(14, 'Didn''t know there were staff advisors', 10)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(15, 'agree strongly', 13)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(16, 'agree', 13)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(17, 'disagree', 13)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(18, 'Yes', 15)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(19, 'No', 15)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(20, 'Didn''t know there were faculty advisors', 15)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(21, 'disagree strongly', 13)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(22, 'neutral', 13)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(23, 'agree strongly', 16)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(24, 'agree', 16)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(25, 'disagree', 16)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(26, 'disagree strongly', 16)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(28, 'neutral', 16)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(29, 'Yes', 17)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(30, 'No', 17)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(31, 'Didn''t know there were staff advisors', 17)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(33, 'agree strongly', 19)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(34, 'agree', 19)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(35, 'disagree', 19)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(36, 'disagree strongly', 19)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(37, 'neutral', 19)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(38, 'agree strongly', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(39, 'agree', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(40, 'disagree', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(41, 'dissagree strongly', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(42, 'neutral', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(43, 'not applicable/don''t know', 21)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(44, 'Expectations fully met', 22)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(45, 'Well', 22)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(46, 'Adequately', 22)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(47, 'Not Well', 22)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(48, 'Not at all', 22)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(49, 'agree strongly', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(50, 'agree', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(51, 'disagree', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(52, 'disagree strongly', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(53, 'neutral', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(54, 'not applicable/don''t know', 23)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(55, 'agree strongly', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(56, 'agree', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(57, 'disagree', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(58, 'disagree strongly', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(59, 'neutral', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(60, 'not applicable/don''t know', 26)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(61, 'Cal Aggie Alumni Association', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(62, 'CA&ES Almuni Giving Club', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(63, 'Facebook', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(64, 'LinkedIn', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(65, 'Twitter', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(66, 'CA&ES Web pages for Alumni', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(67, 'CA&ES Newsletter', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(68, 'UC Davis Events (Picnic Day, Whole Earth Festival, Homecoming)', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(69, 'CA&ES Events (College Celebration/Alumni Awards)', 36)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(70, 'I have used the academic advising provided by the College of Agricultural & Environmental Sciences and/or its departmental major offices.  (Select yes/no for each category)', 7)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(71, 'Yes', 37)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(72, 'No', 37)
INSERT INTO [dbo].[SurveyFieldOptions]([Id], [Name], [SurveyFieldId]) VALUES(73, 'Didn''t know there was a college website', 37)

GO
SET IDENTITY_INSERT [dbo].[SurveyFieldOptions] OFF
GO

INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(17, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(20, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(21, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(22, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(23, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(26, 1)
INSERT INTO [dbo].[SurveyFieldXSurveyFieldValidators]([SurveyFieldId], [SurveyFieldValidatorId]) VALUES(37, 1)


--/*
--Insert the initial survey info, replace 15 with the proper field id
--*/
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Agricultural Management & Rangeland Resources', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Agricultural & Environmental Education', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Biology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Science', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Animal Science & Management', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Atmospheric Science', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Avian Sciences', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Biotechnology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Clinical Nutrition', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Community & Regional Development', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Crop Science & Management', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Ecological Management & Restoration', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Entomology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Biology & Management', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Horticulture & Urban Forestry', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Policy, Analysis, & Planning', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental & Resource Sciences', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Science & Management', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Environmental Toxicology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Fiber & Polymer Science', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Food Science', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Human Development', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Hydrology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('International Agricultural Development', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Landscape Architecture', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Managerial Economics', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Nutrition Science', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Plant Sciences', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Sustainable Agriculture & Food Systems', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Textiles & Clothing', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Viticulture & Enology', 15)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Wildlife, Fish & Conservation Biology', 15)

--insert into SurveyFieldOptions (name, surveyfieldid) values ('Job / Career in Agriculture (no further education)', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Job / Career in the Environmental Sciences (no further education)', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Job with a U.S. government agency', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('K-12 teaching', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Community college/university teaching', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Medicine', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Veterinary Medicine', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Pharmacy', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Dentistry', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Optometry', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Nursing School', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Physical Therapy', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Biotechnology', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Masters program', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Ph.D. program', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Masters program in another discipline', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Ph.D. program in another discipline', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Law school', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Business school', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Undecided', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Combination of the above (Please explain below)', 16)
--insert into SurveyFieldOptions (name, surveyfieldid) values ('Other (Please explain below)', 16)

