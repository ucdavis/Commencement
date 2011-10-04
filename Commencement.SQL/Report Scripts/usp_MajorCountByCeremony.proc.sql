CREATE PROCEDURE [dbo].[usp_MajorCountByCeremony]
	
	@ceremonyId int,
	@userid int

AS

select count(rp.id) num, m.name, c.datetime
from registrationparticipations rp
	inner join registrations r on r.id = rp.registrationid
	inner join students s on r.student_id = s.id
	inner join majors m on rp.majorcode = m.id
	inner join ceremonies c on rp.ceremonyid = c.id
where rp.ceremonyid = @ceremonyId
  and cancelled = 0
  and sjablock = 0 and blocked = 0
group by m.name, c.datetime
order by m.name

RETURN 0