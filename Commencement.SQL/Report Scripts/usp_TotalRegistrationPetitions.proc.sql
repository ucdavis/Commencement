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

--SELECT     RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name AS Major, Ceremonies.[DateTime] AS CeremonyTime,
--                      RegistrationPetitions.ExceptionReason, 
--                      CASE WHEN RegistrationPetitions.IsPending = 1 THEN 'Pending' WHEN RegistrationPetitions.IsPending = 0 AND 
--                      RegistrationPetitions.IsApproved = 1 THEN 'Approved' ELSE 'Denied' END AS PetitionStatus, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 1 THEN RegistrationPetitions.id END) AS TotalPendingPetitions, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 1 THEN RegistrationPetitions.id END) AS TotalApprovedPetitions, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 0 THEN RegistrationPetitions.id END) AS TotalDeniedPetitions, 
--                      TermCodes.Name AS Term
--from RegistrationPetitions
--	inner join TermCodes on RegistrationPetitions.TermCode = TermCodes.id
--	inner join Majors on Majors.id = RegistrationPetitions.MajorCode
--	inner join Ceremonies on Ceremonies.id = RegistrationPetitions.CeremonyId
--WHERE RegistrationPetitions.CeremonyId in 
--		(select CeremonyId from Ceremonies
--			inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--		where UserId = @userid
--			and TermCode = @term)
--GROUP BY RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name, Ceremonies.DateTime, 
--                      RegistrationPetitions.ExceptionReason, RegistrationPetitions.IsPending, RegistrationPetitions.IsApproved, TermCodes.Name

RETURN 0