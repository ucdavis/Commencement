
CREATE Procedure usp_LookupStudentName
	(
		@studentid varchar(9)
	)

AS

	IF object_id('tempdb..#StudentNames') IS NOT NULL
	BEGIN
		DROP TABLE #StudentNames
	END

	create table #StudentNames
	(
		pidm varchar(8),
		firstname varchar(50),
		mi varchar(50),
		lastname varchar(50),
		loginid varchar(50),
		email varchar(50)
	)

	declare @tsql varchar(max)

	set @tsql = '
	
		insert into #studentnames (pidm, firstname, mi, lastname, loginid, email)
		select spriden_pidm, spriden_first_name, spriden_mi, spriden_last_name, loginid, email from openquery (sis, ''
		
			select spriden_pidm, spriden_first_name, spriden_mi, spriden_last_name, lower(wormoth_login_id) as loginid, email.goremal_email_address as email
			from spriden
				left outer join wormoth on wormoth_pidm = spriden_pidm
				left outer join (
					select goremal_pidm, goremal_email_address
					from goremal
					where goremal_emal_code = ''''UCD''''
					  and goremal_status_ind = ''''A''''
				) email on email.goremal_pidm = spriden_pidm
			where spriden_change_ind is null
			  and spriden_id = ''''' + @studentid + '''''

		'')

	'

	exec(@tsql)

	select * from #StudentNames

	drop table #StudentNames