insert into colleges (id, name)
select stvcoll_code, stvcoll_desc  from openquery (sis, '
	select stvcoll_code, stvcoll_desc
	from stvcoll
')
go
insert into majors (id, name, disciplinecode, collegecode)
select distinct stvmajr_code, stvmajr_desc, stvmajr_dspc_code, sorxcur_coll_code
from openquery (sis, '
	select stvmajr_code, stvmajr_desc, stvmajr_dspc_code, sorxcur_coll_code
	from stvmajr
		left outer join sorxcur on stvmajr_code = sorxcur_majr_code
	')
	go