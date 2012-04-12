insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (1234567, 123456789, 'Philip', 'Fry', 200, 'anlai@ucdavis.edu', 'pjfry', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (1738488, 123456788, 'Bender', 'Rodriguez', 188, 'anlai@ucdavis.edu', 'bbrodriguez', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (9854939, 123456787, 'Turanga', 'Leela', 155, 'anlai@ucdavis.edu', 'tleela', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (58493748, 123456778, 'Amy', 'Wong', 167, 'anlai@ucdavis.edu', 'awong', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (0293494, 123456798, 'Hermes', 'Conrad', 177, 'anlai@ucdavis.edu', 'hconrad', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (7593920, 123456677, 'John', 'Zoidberg', 198, 'anlai@ucdavis.edu', 'jzoid', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (5893029, 123456678, 'Cubert', 'Farnsworth', 190, 'anlai@ucdavis.edu', 'cfarns', '201010')

insert into Students (Pidm, StudentId, FirstName, LastName, Units, Email, [Login], TermCode)
values (1829503, 123456679, 'Kif', 'Kroker', 158, 'anlai@ucdavis.edu', 'kkroker', '201010')

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