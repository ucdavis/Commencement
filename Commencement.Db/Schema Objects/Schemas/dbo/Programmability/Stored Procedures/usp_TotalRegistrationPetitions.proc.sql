
CREATE PROCEDURE [dbo].[usp_TotalRegistrationPetitions]
	@term varchar(6),
	@userid int
AS
	
select students.LastName, students.FirstName, students.StudentId, rp.MajorCode Major
	, Ceremonies.[DateTime] as CeremonyTime
	, rp.ExceptionReason
	, case when rp.IsPending = 1 then 'Pending' when rp.IsPending = 0 and rp.IsApproved = 1 then 'Approved'
		else 'Denied'
	  end as PetitionStatus
	, COUNT(case when rp.IsPending = 1 then rp.id end) as TotalPendingPetitions	
	, COUNT(case when rp.IsPending = 0 and rp.IsApproved = 1 then rp.id end) as TotalApprovedPetitions
	, COUNT(case when rp.IsPending = 0 and rp.IsApproved = 0 then rp.id end) as TotalDeniedPetitions
	, TermCodes.Name as Term
from RegistrationPetitions rp
	inner join Registrations reg on rp.RegistrationId = reg.id
	inner join Students on students.Id = reg.Student_Id
	inner join Ceremonies on Ceremonies.id = rp.CeremonyId
	inner join TermCodes on students.termcode = termcodes.id
where students.TermCode = @term
  and students.SJABlock = 0
  and rp.CeremonyId in ( select CeremonyId from Ceremonies
							inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
						 where UserId = @userid
						   and TermCode = @term)						   
group by students.LastName, students.FirstName, students.StudentId, rp.MajorCode, Ceremonies.[DateTime]
	, rp.ExceptionReason, rp.IsPending, rp.IsApproved, TermCodes.Name
order by students.LastName

RETURN 0