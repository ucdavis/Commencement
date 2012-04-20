using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;

namespace Commencement.Controllers.Helpers
{
    public interface IRegistrationPopulator
    {
        Registration PopulateRegistration(RegistrationPostModel registrationPostModel, Student student, ModelStateDictionary modelState, bool adminUpdate = false);
        void UpdateRegistration(Registration registration, RegistrationPostModel registrationPostModel, Student student, ModelStateDictionary modelState, bool adminUpdate = false);
    }

    public class RegistrationPopulator : IRegistrationPopulator
    {
        private readonly IRepository<RegistrationParticipation> _participationRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<SpecialNeed> _specialNeedsRepository;
        private readonly IRepository<RegistrationPetition> _registrationPetitionRepository;

        public RegistrationPopulator(IRepository<SpecialNeed> specialNeedsRepository, IRepository<RegistrationPetition> registrationPetitionRepository, IRepository<RegistrationParticipation> participationRepository, IRepository<Registration> registrationRepository)
        {
            _participationRepository = participationRepository;
            _registrationRepository = registrationRepository;
            _specialNeedsRepository = specialNeedsRepository;
            _registrationPetitionRepository = registrationPetitionRepository;
        }

        public Registration PopulateRegistration(RegistrationPostModel registrationPostModel, Student student, ModelStateDictionary modelState, bool adminUpdate = false)
        {
            // setup variables to be used for this method
            var ceremonyParticipations = registrationPostModel.CeremonyParticipations;
            var specialNeeds = registrationPostModel.SpecialNeeds;
            var term = TermService.GetCurrent();

            var registration = registrationPostModel.Registration;
            registration.TermCode = term;
            registration.Student = student;
            NullOutBlankFields(registration);
            registration.SpecialNeeds = LoadSpecialNeeds(specialNeeds);
            registration.GradTrack = registrationPostModel.GradTrack;

            //ValidateCeremonyParticipations(ceremonyParticipations, modelState);
            AddCeremonyParticipations(registration, ceremonyParticipations, modelState, adminUpdate);
            AddRegistrationPetitions(registration, ceremonyParticipations, modelState);

            return registration;
        }

        public void UpdateRegistration(Registration registration, RegistrationPostModel registrationPostModel, Student student, ModelStateDictionary modelState, bool adminUpdate = false)
        {
            CopyHelper.CopyRegistrationValues(registrationPostModel.Registration, registration);
            registration.GradTrack = registrationPostModel.GradTrack;

            NullOutBlankFields(registration);
            registration.SpecialNeeds = LoadSpecialNeeds(registrationPostModel.SpecialNeeds);
            UpdateCeremonyParticipations(registration, registrationPostModel.CeremonyParticipations, modelState, adminUpdate);
            AddRegistrationPetitions(registration, registrationPostModel.CeremonyParticipations, modelState);
        }

        private void NullOutBlankFields(Registration registration)
        {
            registration.Address2 = registration.Address2.IsNullOrEmpty(true) ? null : registration.Address2;
            registration.Email = registration.Email.IsNullOrEmpty(true) ? null : registration.Email;
        }

        private List<SpecialNeed> LoadSpecialNeeds(List<string> specialNeeds)
        {
            if (specialNeeds == null) return new List<SpecialNeed>();

            var needs = new List<int>();
            foreach (var a in specialNeeds)
            {
                if(!string.IsNullOrEmpty(a)) needs.Add(Convert.ToInt32(a));
            }
            return _specialNeedsRepository.Queryable.Where(a => needs.Contains(a.Id)).ToList();
        }

        private void AddCeremonyParticipations(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState, bool adminUpdate = false)
        {
            ValidateCeremonyParticipations(ceremonyParticipations, modelState);

            foreach (var a in ceremonyParticipations)
            {
                if (a.Participate && (adminUpdate || a.Ceremony.CanRegister()))
                {
                    registration.AddParticipation(a.Major, a.Ceremony, a.Tickets, a.TicketDistributionMethod);
                }
            }
        }

        private void UpdateCeremonyParticipations(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState, bool adminUpdate = false)
        {
            ValidateCeremonyParticipations(ceremonyParticipations, modelState);

            foreach (var a in ceremonyParticipations)
            {
                var rp = registration.RegistrationParticipations.Where(b => b.Id == a.ParticipationId).SingleOrDefault();

                // only allow updates within registration times or during an admin update
                if (adminUpdate || a.Ceremony.CanRegister())
                {
                    // case where we are newly registering
                    if (rp == null && a.Participate && !a.Cancel) registration.AddParticipation(a.Major, a.Ceremony, a.Tickets, a.TicketDistributionMethod);
                    // case where we are cancelling
                    else if (rp != null)
                    {
                        rp.Cancelled = a.Participate && !a.Cancel ? !a.Participate : a.Cancel;
                        rp.DateUpdated = DateTime.Now;
                        rp.NumberTickets = a.Tickets;
                        rp.TicketDistributionMethod = a.TicketDistributionMethod;

                        if (adminUpdate)
                        {
                            rp.Major = a.Major;
                            rp.Ceremony = a.Ceremony;
                        }
                    }
                }
            }
        }

        private void AddRegistrationPetitions(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            //var petitions = new List<RegistrationPetition>();

            foreach (var a in ceremonyParticipations.Where(a => a.Petition))
            {
                // check for existing petition
                var noExistingPetition = !_registrationPetitionRepository.Queryable.Any(b => b.Ceremony == a.Ceremony && b.Registration.Student == registration.Student);
                // check for existing registration
                var noExistingParticipation = !_participationRepository.Queryable.Any(b => b.Ceremony == a.Ceremony && b.Registration.Student == registration.Student);

                if (noExistingPetition && noExistingParticipation && a.Ceremony.CanRegister())
                {
                    var petition = new RegistrationPetition(registration, a.Major, a.Ceremony, a.PetitionReason, a.CompletionTerm, a.Tickets);
                    petition.TransferUnitsFrom = string.IsNullOrEmpty(a.TransferCollege) ? null : a.TransferCollege;
                    petition.TransferUnits = string.IsNullOrEmpty(a.TransferUnits) ? null : a.TransferUnits;
                    petition.TicketDistributionMethod = a.TicketDistributionMethod;

                    registration.AddPetition(petition);
                }
            }

            //return petitions;
        }

        /// <summary>
        /// Validates that the ceremonies the student has decided upon are all valid to be registered for together
        /// </summary>
        /// <remarks>
        /// Rules are:
        ///     Student can register once per ceremony
        ///     Student can register once per college
        ///     Student can register for multiple ceremonies for different colleges
        /// </remarks>
        /// <param name="registration"></param>
        /// <param name="ceremonyParticipations"></param>
        private void ValidateCeremonyParticipations(List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            // count distinct ceremonies that student has selected
            var ceremonyCount = ceremonyParticipations.Where(a => a.Participate || a.Cancel).Select(a => a.Ceremony).Distinct();
            var petitionCount = ceremonyParticipations.Where(a => a.Petition).Select(a => a.Ceremony).Distinct();
            if (ceremonyCount.Count() == 0 && petitionCount.Count() == 0)
            {
                modelState.AddModelError("Participate", "You have to select one or more ceremonies to participate.");
            }

            // if # participating != distinct, then we have someone registering more than once for the same ceremony
            if (ceremonyParticipations.Where(a => a.Participate || a.Petition).Count() > ceremonyCount.Count() + petitionCount.Count())
            {
                modelState.AddModelError("Participate", "You cannot register for two majors within the same ceremony.");
            }

            // count distinct colleges
            var collegeCount = ceremonyParticipations.Where(a => a.Participate || a.Petition).Select(a => a.Major.MajorCollege).Distinct();
            // if # participating != distinct, then we have someone registering for more than on ceremony in one college
            if (ceremonyParticipations.Where(a => a.Participate || a.Petition).Count() > collegeCount.Count())
            {
                modelState.AddModelError("Participate", "You cannot register for two ceremonies within the same college.");
            }
        }
    }

    public class RegistrationPostModel
    {
        public Registration Registration { get; set; }
        public List<CeremonyParticipation> CeremonyParticipations { get; set; }
        public List<string> SpecialNeeds { get; set; }
        public bool AgreeToDisclaimer { get; set; }
        public bool GradTrack { get; set; }
    }
}