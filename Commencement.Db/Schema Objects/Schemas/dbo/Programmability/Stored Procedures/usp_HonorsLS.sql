CREATE PROCEDURE [dbo].[usp_HonorsLS]

	@term varchar(6),	
	@honors_4590 decimal (18,3),
	@honors_90135 decimal (18,3),
	@honors_135 decimal (18,3)

AS

declare 
	@coll char(2) = 'LS',
	@tsql varchar(max),
	@awarded varchar(max),
	@candidate varchar(max)


set @awarded = '
	select spriden_id as StudentId, spriden_first_name as FirstName, spriden_mi as MiddleName, spriden_last_name as LastName, stvhond_desc HonorsLevel
		, stvmajr_desc as MajorDescription
	from spriden
	inner join (

		select shrdgmr_pidm as pidm, shrdgmr_majr_code_1 as major, shrdgmr_hond_code_majr_code1 as honorslevel
		from shrdgmr
		where shrdgmr_term_code_grad = ' + @term + '
			and shrdgmr_levl_code = ''''UG''''
			and shrdgmr_degs_code = ''''DA''''
			and shrdgmr_coll_code_1 = '''''+@coll+'''''
			and shrdgmr_hond_code_majr_code1 is not null

		union

		select shrdgmr_pidm as pidm, shrdgmr_majr_code_1_2 as major, shrdgmr_hond_code_majr_code1 as honorslevel
		from shrdgmr
		where shrdgmr_term_code_grad = ' + @term + '
			and shrdgmr_levl_code = ''''UG''''
			and shrdgmr_degs_code = ''''DA''''
			and shrdgmr_coll_code_1 = '''''+@coll+'''''
			and shrdgmr_hond_code_majr_code1_2 is not null

		union

		select shrdgmr_pidm as pidm, shrdgmr_majr_code_1 as major, shrdgmr_hond_code_majr_code1 as honorslevel
		from shrdgmr
		where shrdgmr_term_code_grad = ' + @term + '
			and shrdgmr_levl_code = ''''UG''''
			and shrdgmr_degs_code = ''''DA''''
			and shrdgmr_coll_code_2 = '''''+@coll+'''''
			and shrdgmr_hond_code_majr_code2 is not null

		union

		select shrdgmr_pidm as pidm, shrdgmr_majr_code_1_2 as major, shrdgmr_hond_code_majr_code1 as honorslevel
		from shrdgmr
		where shrdgmr_term_code_grad = ' + @term + '
			and shrdgmr_levl_code = ''''UG''''
			and shrdgmr_degs_code = ''''DA''''
			and shrdgmr_coll_code_2 = '''''+@coll+'''''
			and shrdgmr_hond_code_majr_code2_2 is not null

	) HonorsAwarded on spriden_pidm = HonorsAwarded.Pidm
		inner join stvmajr on honorsawarded.major = stvmajr.stvmajr_code
		inner join stvhond on honorsawarded.honorslevel = stvhond_stvhond_code
	where spriden_change_ind is null
'

set @candidate = '
	select spriden_id as StudentId, spriden_first_name as FirstName, spriden_mi as MiddleName, spriden_last_name as LastName
		, ''''Candidate for Honors'''' as HonorsLevel
		, ''''All L&S Majors'''' as MajorDescription
	from spriden
		inner join zhvlcfs on spriden_pidm = zhvlcfs_pidm
		inner join (
			select shrlgpa_pidm pidm
				, sum(shrlgpa_quality_points) / sum(shrlgpa_gpa_hours) uc_gpa
				, sum(shrlgpa_hours_earned) uc_hours
				from shrlgpa
				where shrlgpa_gpa_type_ind in (''''I'''', ''''U'''')
				group by shrlgpa_pidm
				having sum(shrlgpa_gpa_hours) > 0
		) Grades on Grades.Pidm = zhvlcfs_pidm
	where zhvlcfs_coll_code = ''''LS''''
		and zhvlcfs_degs_code = ''''FG''''
		and zhvlcfs_levl_code = ''''UG''''
		and zhvlcfs_term_code_grad = '+@term+'
		and grades.uc_gpa >= case when grades.uc_hours >= 45 and grades.uc_hours > 90 then '+cast(@honors_4590 as varchar(10))+'
								when grades.uc_hours >= 90 and grades.uc_hours < 135 then '+cast(@honors_90135 as varchar(10))+'
								when grades.uc_hours >= 135 then '+cast(@honors_135 as varchar(10))+'
								else 9.999
							end
'

set @tsql = '

	select * from openquery (sis, ''

	'+@awarded+'

	union

	'+@candidate+'

	'')

'

exec (@tsql)



RETURN 0
