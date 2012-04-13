declare @term varchar(6) = '201203'

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (1234567, 123456789, 'Philip', 'Fry', 200, 0, 'fake@ucdavis.edu', 'pjfry', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (1738488, 123456788, 'Bender', 'Rodriguez', 188, 0, 'fake@ucdavis.edu', 'bbrodriguez', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (9854939, 123456787, 'Turanga', 'Leela', 155, 0, 'fake@ucdavis.edu', 'tleela', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (58493748, 123456778, 'Amy', 'Wong', 167, 0, 'fake@ucdavis.edu', 'awong', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (0293494, 123456798, 'Hermes', 'Conrad', 177, 0, 'fake@ucdavis.edu', 'hconrad', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (7593920, 123456677, 'John', 'Zoidberg', 198, 0, 'fake@ucdavis.edu', 'jzoid', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (5893029, 123456678, 'Cubert', 'Farnsworth', 190, 0, 'fake@ucdavis.edu', 'cfarns', @term)

insert into Students (Pidm, StudentId, FirstName, LastName, EarnedUnits, CurrentUnits, Email, [Login], TermCode)
values (1829503, 123456679, 'Kif', 'Kroker', 158, 0, 'fake@ucdavis.edu', 'kkroker', @term)

declare @sid uniqueidentifier
select @sid = id from students where pidm = 1234567
insert into studentmajors (student_id, majorcode) values (@sid, 'AABI')
insert into studentmajors (student_id, majorcode) values (@sid, 'LANT')

select @sid = id from students where pidm = 1738488
insert into studentmajors (student_id, majorcode) values (@sid, 'ABIS')

select @sid = id from students where pidm = 9854939
insert into studentmajors (student_id, majorcode) values (@sid, 'ACBI')
insert into studentmajors (student_id, majorcode) values (@sid, 'EAER')

select @sid = id from students where pidm = 58493748
insert into studentmajors (student_id, majorcode) values (@sid, 'ACRP')
insert into studentmajors (student_id, majorcode) values (@sid, 'EBAF')

select @sid = id from students where pidm = 0293494
insert into studentmajors (student_id, majorcode) values (@sid, 'LANT')

select @sid = id from students where pidm = 7593920
insert into studentmajors (student_id, majorcode) values (@sid, 'EAER')

select @sid = id from students where pidm = 5893029
insert into studentmajors (student_id, majorcode) values (@sid, 'EBAF')

select @sid = id from students where pidm = 1829503
insert into studentmajors (student_id, majorcode) values (@sid, 'ECIM')
insert into studentmajors (student_id, majorcode) values (@sid, 'LANT')

select studentid, firstname, lastname, email, login, MajorCode
from students
	inner join studentmajors on student_id = students.id
where pidm in (1234567, 1738488, 9854939, 58493748, 0293494, 7593920, 5893029, 1829503)