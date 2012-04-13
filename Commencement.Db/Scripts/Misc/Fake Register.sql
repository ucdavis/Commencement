declare @majors cursor, @majorcode varchar(4)
declare @temp table (id uniqueidentifier)

declare @ceremonyId int = 5
declare @college varchar(2) = 'AE'
declare @term varchar(6) = '201203'

set @majors = cursor for
	select majorcode from ceremonymajors where ceremonyid = @ceremonyid

open @majors

fetch next from @majors into @majorcode

while(@@FETCH_STATUS = 0)
begin

	delete from @temp
	insert into @temp (id) 
		select top(10)percent student_id 
		from studentmajors 
		where majorcode = @majorcode 
			and student_id in ( select id from students where termcode = @term )
			and student_id not in ( select student_id from registrations where termcode = @term )
	
	-- insert the registration
	insert into registrations (student_id, address1, city, state, zip, termcode, gradtrack)
	select temp.id, cast(floor(rand(convert(varbinary, newid())) * 10000) as varchar(10)) + ' Fake Street' address1
		, 'davis' city, 'CA' state, '95616' zip
		, @term termcode
		, cast(floor((rand(convert(varbinary, newid())) *10000)) as int) % 2 gradtrack
	from @temp temp

	-- insert the participations
	insert into RegistrationParticipations (registrationid, majorcode, ceremonyid, numbertickets, labelprinted, dateregistered, dateupdated, ticketdistributionmethodid)
	select registrations.id, @majorcode, @ceremonyid, (cast(floor((rand(convert(varbinary, newid())) *10000)) as int) % 9) + 1
		, 0, getdate(), getdate()
		, (select id from (select top(1) *, newid() sortkey from ticketdistributionmethods order by sortkey) td)
	from registrations
	where student_id in ( select id from @temp)
	  and termcode = @term

	fetch next from @majors into @majorcode

end

close @majors
deallocate @majors

