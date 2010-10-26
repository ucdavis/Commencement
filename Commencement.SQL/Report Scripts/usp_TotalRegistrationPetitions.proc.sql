IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_TotalRegistrationPetitions')
    BEGIN
        DROP  Procedure  usp_TotalRegistrationPetitions
    END

GO

CREATE PROCEDURE [dbo].[usp_TotalRegistrationPetitions]
	@term varchar(6),
	@userid int
AS
	
SELECT     RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name AS Major, Ceremonies.[DateTime] AS CeremonyTime,
                      RegistrationPetitions.ExceptionReason, 
                      CASE WHEN RegistrationPetitions.IsPending = 1 THEN 'Pending' WHEN RegistrationPetitions.IsPending = 0 AND 
                      RegistrationPetitions.IsApproved = 1 THEN 'Approved' ELSE 'Denied' END AS PetitionStatus, 
                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 1 THEN RegistrationPetitions.id END) AS TotalPendingPetitions, 
                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 1 THEN RegistrationPetitions.id END) AS TotalApprovedPetitions, 
                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 0 THEN RegistrationPetitions.id END) AS TotalDeniedPetitions, 
                      TermCodes.Name AS Term
from RegistrationPetitions
	inner join TermCodes on RegistrationPetitions.TermCode = TermCodes.id
	inner join Majors on Majors.id = RegistrationPetitions.MajorCode
	inner join Ceremonies on Ceremonies.id = RegistrationPetitions.CeremonyId
WHERE RegistrationPetitions.CeremonyId in 
		(select CeremonyId from Ceremonies
			inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
		where UserId = @userid
			and TermCode = @term)
GROUP BY RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name, Ceremonies.DateTime, 
                      RegistrationPetitions.ExceptionReason, RegistrationPetitions.IsPending, RegistrationPetitions.IsApproved, TermCodes.Name

RETURN 0