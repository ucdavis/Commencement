﻿--CREATE PROCEDURE [dbo].[usp_DownloadMajors]

--AS

--merge majors as t
--using
--	(select stvmajr_code, stvmajr_desc, stvmajr_dspc_code
--		, case when stvmajr_code like 'A%' then 'AE'
--			   when stvmajr_code like 'B%' then 'BI'
--			   when stvmajr_code like 'E%' then 'EN'
--			   when stvmajr_code like 'L%' then 'LS'
--			   else NULL end as collegecode
--	from openquery (sis, '
--		select stvmajr_code, stvmajr_desc, stvmajr_valid_major_ind, stvmajr_dspc_code
--		from stvmajr
--		where stvmajr_valid_major_ind = ''Y''
--		  and (
--			stvmajr_dspc_code in (''HARCS'', ''MTHPS'', ''SOCSC'', ''AGRSC'', ''ENVSC'', ''HUMSC'', ''ENGIN'', ''BIOSC'')
--			or stvmajr_code in (''LIND'', ''AIND'', ''BIND'', ''GINP'')
--			or stvmajr_code in (''AIAD'', ''ASAF'')
--		  )
--		order by stvmajr_code
--	')
--	) as s
--on t.id = s.stvmajr_code
--when matched then update set
--	t.name = s.stvmajr_desc, t.disciplinecode = s.stvmajr_dspc_code, t.collegecode = s.collegecode
--when not matched by source then update set
--	t.isactive = 0
--when not matched then
--	insert (id, name, disciplinecode, collegecode, isactive) values (s.stvmajr_code, s.stvmajr_desc, s.stvmajr_dspc_code, s.collegecode, 1);

--RETURN 0
