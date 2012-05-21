CREATE PROCEDURE [dbo].[usp_Honors]

	@term varchar(6),	
	@coll char(2),
	@honors_4590 decimal (18,3),
	@honors_90135 decimal (18,3),
	@honors_135 decimal (18,3),
	@highhonors_4590 decimal (18,3),
	@highhonors_90135 decimal (18,3),
	@highhonors_135 decimal (18,3),
	@highesthonors_4590 decimal (18,3),
	@highesthonors_90135 decimal (18,3),
	@highesthonors_135 decimal (18,3)

AS

declare @tsql varchar(max), @awarded varchar(max), @candidate varchar(max)

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

select spriden_id, spriden_last_name, spriden_first_name, spriden_mi
		, grades.uc_hours, round(grades.uc_gpa, 3) uc_gpa, zhvlcfs_majr_code
		, stvhond_desc
from zhvlcfs
	inner join spriden on spriden_pidm = zhvlcfs_pidm
	left outer join (
		select shrlgpa_pidm pidm
			, sum(shrlgpa_quality_points) / sum(shrlgpa_gpa_hours) uc_gpa
			, sum(shrlgpa_hours_earned) uc_hours
			from shrlgpa
			where shrlgpa_gpa_type_ind in (''''I'''', ''''U'''')
			group by shrlgpa_pidm
			having sum(shrlgpa_gpa_hours) > 0
	) Grades on Grades.Pidm = zhvlcfs_pidm
	, stvhond
where zhvlcfs_coll_code = '''''+@coll+'''''
	and zhvlcfs_degs_code = ''''FG''''
	and zhvlcfs_term_code_grad = ''''' + @term + '''''
	and spriden_change_ind is null
	and case 
		when zhvlcfs_coll_code = '''''+@coll+''''' and  zhvlcfs_majr_code in (''''ABMB'''', ''''ABIS'''', ''''ACBI'''', ''''AEEB'''', ''''AGEN'''', ''''AMIC'''', ''''ANPB'''', ''''APLB'''', ''''AEVE'''', ''''BEEB'''') then 0
		else 1
		end = 1
	and case
		when (CAST(uc_hours as float) >= 45.0 and CAST(uc_hours as float) < 90.0 and CAST(uc_gpa as float) >= '+cast(@highesthonors_4590 as varchar(10))+') then 3
		when (CAST(uc_hours as float) >= 45.0 and CAST(uc_hours as float) < 90.0 and CAST(uc_gpa as float) >= '+cast(@highhonors_4590 as varchar(10))+') then 2
		when (CAST(uc_hours as float) >= 45.0 and CAST(uc_hours as float) < 90.0 and CAST(uc_gpa as float) >= '+cast(@honors_4590 as varchar(10))+') then 1
		when (CAST(uc_hours as float) >= 90.0 and CAST(uc_hours as float) < 135.0 and CAST(uc_gpa as float) >= '+cast(@highesthonors_90135 as varchar(10))+') then 3
		when (CAST(uc_hours as float) >= 90.0 and CAST(uc_hours as float) < 135.0 and CAST(uc_gpa as float) >= '+cast(@highhonors_90135 as varchar(10))+') then 2
		when (CAST(uc_hours as float) >= 90.0 and CAST(uc_hours as float) < 135.0 and CAST(uc_gpa as float) >= '+cast(@honors_90135 as varchar(10))+') then 1
		when (CAST(uc_hours as float) >= 135.0 and CAST(uc_gpa as float) >= '+cast(@highesthonors_135 as varchar(10))+') then 3
		when (CAST(uc_hours as float) >= 135.0 and CAST(uc_gpa as float) >= '+cast(@highhonors_135 as varchar(10))+') then 2
		when (CAST(uc_hours as float) >= 135.0 and CAST(uc_gpa as float) >= '+cast(@honors_135 as varchar(10))+') then 1
		else 0
		end = stvhond_code
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
