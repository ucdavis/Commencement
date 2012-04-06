-- =============================================
-- Insert needed data for move major template
-- =============================================

insert into TemplateTypes(Name, [Description], Code)
values ('Move Major', 'E-Mail sent to student when their major is moved to a new ceremony.', 'MM')

declare @tid int, @rid int

select @tid = id from TemplateTypes where Code = 'MM'
select @rid = id from TemplateTypes where Code = 'RC'

insert into templatetokens (templatetypeid, name)
select @tid, name from templatetokens where templatetypeid = @rid