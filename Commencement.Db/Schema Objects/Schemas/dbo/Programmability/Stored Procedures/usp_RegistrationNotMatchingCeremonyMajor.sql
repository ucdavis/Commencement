CREATE PROCEDURE [dbo].[usp_RegistrationMajorMismatch]
	@term varchar(6),
	@userid int
AS
	
	select s.studentid, s.lastname, s.firstname, rp.MajorCode
		, c.name
	from RegistrationParticipations rp
		inner join Ceremonies c on rp.CeremonyId = c.Id
		inner join registrations r on rp.registrationid = r.id
		inner join Students s on r.student_id = s.id
	where c.TermCode = @term
	  and rp.ceremonyid in (select ceremonyid from ceremonyeditors where userid = @userid)
	  and rp.majorcode not in ( select majorcode from CeremonyMajors cm where cm.CeremonyId = c.id )

RETURN 0
