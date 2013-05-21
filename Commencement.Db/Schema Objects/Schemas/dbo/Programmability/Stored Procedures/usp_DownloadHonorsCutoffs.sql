/*

SSDT has trouble compiling this, so this needs to be added manually

*/

CREATE PROCEDURE [dbo].[usp_DownloadHonorsCutoffs]
AS

--merge HonorsCutoffs as t
--using (
--	select data.zorhond_coll_code as College
--		, data.zorhond_term_code as StartTerm
--		, data.zorhond_hours_minimum as MinUnits
--		, data.HonorsGpa as HonorsGpa
--		, data.HighHonorsGpa as HighHonorsGpa
--		, data.HighestHonorsGpa as HighestHonorsGpa
--	from openquery (sis, '
--		select
--			zorhond_coll_code
--			, zorhond_term_code
--			, zorhond_hours_minimum
--			, zorhond_cutoff_gpa as HonorsGpa
--			, case zorhond_coll_code when ''LS'' then null
--				else (
--				select zorhond_cutoff_gpa
--				from zorhond zorhond2
--				where zorhond2.zorhond_term_code = zorhond.zorhond_term_code
--					and zorhond2.zorhond_coll_code = zorhond.zorhond_coll_code
--					and zorhond2.zorhond_hours_minimum = zorhond.zorhond_hours_minimum
--					and zorhond2.zorhond_hond_code = 2
--				) end as HighHonorsGpa
--			, case zorhond_coll_code when ''LS'' then null
--				else (
--				select zorhond_cutoff_gpa
--				from zorhond zorhond2
--				where zorhond2.zorhond_term_code = zorhond.zorhond_term_code
--					and zorhond2.zorhond_coll_code = zorhond.zorhond_coll_code
--					and zorhond2.zorhond_hours_minimum = zorhond.zorhond_hours_minimum
--					and zorhond2.zorhond_hond_code = 3
--				) end as HighestHonorsGpa
--		from zorhond
--		where zorhond_term_code = (select max(zorhond_term_code) from zorhond)
--			and zorhond_hond_code = 1
--		') as data
--	) as s
--on (t.StartTerm = s.StartTerm and t.College = s.College and t.MinUnits = s.MinUnits)
--when matched then
--	update set HonorsGpa = s.HonorsGpa, HighHonorsGpa = s.HighHonorsGpa, HighestHonorsGpa = s.HighestHonorsGpa
--when not matched then 
--	insert (College, StartTerm, MinUnits, HonorsGpa, HighHonorsGpa, HighestHonorsGpa)
--	values (s.College, s.StartTerm, s.MinUnits, s.HonorsGpa, s.HighHonorsGpa, s.HighestHonorsGpa);

RETURN 0
