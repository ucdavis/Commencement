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

-- =============================================
-- Insert needed data for email blast "template"
-- =============================================

insert into TemplateTypes(Name, [Description], Code)
values ('Email All Students', 'E-mail sent to all students for a specific ceremony.', 'EA')

insert into TicketDistributionMethods(id, name, isactive) values ('ML', 'Mail Tickets', 1)
insert into TicketDistributionMethods(id, name, isactive) values ('PU', 'Pickup Tickets', 1)