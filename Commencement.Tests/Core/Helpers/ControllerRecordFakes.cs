using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Commencement.Tests.Core.Helpers
{
    /// <summary>
    /// Static methods to fake collections of records for controller tests.
    /// </summary>
    public static class ControllerRecordFakes
    {

        /// <summary>
        /// Fakes the student. Note: Using more than 20 will not be reliable
        /// because the Guids will be random
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="studentRepository">The student repository.</param>
        /// <param name="studentRepository2">The student repository2.</param>
        public static void FakeStudent(int count, IRepositoryWithTypedId<Student, Guid> studentRepository, IRepository<Student> studentRepository2)
        {
            var students = new List<Student>();
            FakeStudent(count, studentRepository, students, studentRepository2);
        }


        /// <summary>
        /// Fakes the student. Note: Using more than 20 will not be reliable
        /// because the Guids will be random
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="studentRepository">The student repository.</param>
        /// <param name="specificStudents">The specific students.</param>
        /// <param name="studentRepository2">The student repository2.</param>
        public static void FakeStudent(int count, IRepositoryWithTypedId<Student, Guid> studentRepository, List<Student> specificStudents, IRepository<Student> studentRepository2)
        {
            var students = new List<Student>();
            var specificStudentsCount = 0;
            if (specificStudents != null)
            {
                specificStudentsCount = specificStudents.Count;
                for (int i = 0; i < specificStudentsCount; i++)
                {
                    students.Add(specificStudents[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                students.Add(CreateValidEntities.Student(i + specificStudentsCount + 1));
            }

            var totalCount = students.Count;
            for (int i = 0; i < totalCount; i++)
            {
                students[i].SetIdTo(SpecificGuid.GetGuid(i+1));
                int i1 = i;
                studentRepository
                    .Expect(a => a.GetNullableById(students[i1].Id))
                    .Return(students[i])
                    .Repeat
                    .Any();
            }
            studentRepository.Expect(a => a.GetNullableById(SpecificGuid.GetGuid(totalCount + 1))).Return(null).Repeat.Any();
            studentRepository.Expect(a => a.Queryable).Return(students.AsQueryable()).Repeat.Any();
            studentRepository.Expect(a => a.GetAll()).Return(students).Repeat.Any();
            if(studentRepository2 != null)
            {
                studentRepository2.Expect(a => a.Queryable).Return(students.AsQueryable()).Repeat.Any();
                studentRepository2.Expect(a => a.GetAll()).Return(students).Repeat.Any();
            }
        }


        /// <summary>
        /// Fakes the registration.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        public static void FakeRegistration(int count, IRepository<Registration> registrationRepository)
        {
            var registrations = new List<Registration>();
            FakeRegistration(count, registrationRepository, registrations);
        }

        /// <summary>
        /// Fakes the registration.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="registrationRepository">The registration repository.</param>
        /// <param name="specificRegistrations">The specific registrations.</param>
        public static void FakeRegistration(int count, IRepository<Registration> registrationRepository, List<Registration> specificRegistrations)
        {
            var registrations = new List<Registration>();
            var specificTransactionsCount = 0;
            if (specificRegistrations != null)
            {
                specificTransactionsCount = specificRegistrations.Count;
                for (int i = 0; i < specificTransactionsCount; i++)
                {
                    registrations.Add(specificRegistrations[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                registrations.Add(CreateValidEntities.Registration(i + specificTransactionsCount + 1));
            }

            var totalCount = registrations.Count;
            for (int i = 0; i < totalCount; i++)
            {
                registrations[i].SetIdTo(i + 1);
                int i1 = i;
                registrationRepository
                    .Expect(a => a.GetNullableById(i1 + 1))
                    .Return(registrations[i])
                    .Repeat
                    .Any();
            }
            registrationRepository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            registrationRepository.Expect(a => a.Queryable).Return(registrations.AsQueryable()).Repeat.Any();
            registrationRepository.Expect(a => a.GetAll()).Return(registrations).Repeat.Any();
        }               

        /// <summary>
        /// Fakes the ceremony.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="ceremonyRepository">The ceremony repository.</param>
        public static void FakeCeremony(int count, IRepository<Ceremony> ceremonyRepository)
        {
            var ceremonies = new List<Ceremony>();
            FakeCeremony(count, ceremonyRepository, ceremonies);
        }

        /// <summary>
        /// Fakes the ceremony.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="ceremonyRepository">The ceremony repository.</param>
        /// <param name="specificCeremonies">The specific ceremonies.</param>
        public static void FakeCeremony(int count, IRepository<Ceremony> ceremonyRepository, List<Ceremony> specificCeremonies)
        {
            var ceremonies = new List<Ceremony>();
            var specificTransactionsCount = 0;
            if (specificCeremonies != null)
            {
                specificTransactionsCount = specificCeremonies.Count;
                for (int i = 0; i < specificTransactionsCount; i++)
                {
                    ceremonies.Add(specificCeremonies[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + specificTransactionsCount + 1));
            }

            var totalCount = ceremonies.Count;
            for (int i = 0; i < totalCount; i++)
            {
                ceremonies[i].SetIdTo(i + 1);
                int i1 = i;
                ceremonyRepository
                    .Expect(a => a.GetNullableById(i1 + 1))
                    .Return(ceremonies[i])
                    .Repeat
                    .Any();
            }
            ceremonyRepository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            ceremonyRepository.Expect(a => a.Queryable).Return(ceremonies.AsQueryable()).Repeat.Any();
            ceremonyRepository.Expect(a => a.GetAll()).Return(ceremonies).Repeat.Any();
        }


        /// <summary>
        /// Fakes the state.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="repository">The repository.</param>
        public static void FakeState(int count, IRepository repository)
        {
            var states = new List<State>();
            FakeState(count, repository, states);
        }

        /// <summary>
        /// Fakes the state.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="specificStates">The specific states.</param>
        public static void FakeState(int count, IRepository repository, List<State> specificStates)
        {
            var states = new List<State>();
            var specificStatesCount = 0;
            if (specificStates != null)
            {
                specificStatesCount = specificStates.Count;
                for (int i = 0; i < specificStatesCount; i++)
                {
                    states.Add(specificStates[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                states.Add(CreateValidEntities.State(i + specificStatesCount + 1));
            }

            var totalCount = states.Count;
            for (int i = 0; i < totalCount; i++)
            {
                states[i].SetIdTo((i + 1).ToString());
                //int i1 = i;
                //repository.OfType<State>()
                //    .Expect(a => a.GetNullableById(i1 + 1))
                //    .Return(states[i])
                //    .Repeat
                //    .Any();
            }
            //State is not an Int Id, if I need to fake this, I'll need to pass a different repository
            //repository.OfType<State>().Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.OfType<State>().Expect(a => a.Queryable).Return(states.AsQueryable()).Repeat.Any();
            repository.OfType<State>().Expect(a => a.GetAll()).Return(states).Repeat.Any();
        }

        public static void FakeTermCode(int count, IRepository<TermCode> termCodeRepository)
        {
            var termCodes = new List<TermCode>();
            FakeTermCode(count, termCodeRepository, termCodes);
        }


        public static void FakeTermCode(int count, IRepository<TermCode> termCodeRepository, List<TermCode> specificTermCodes)
        {
            var termCodes = new List<TermCode>();
            var specificTermCodesCount = 0;
            if (specificTermCodes != null)
            {
                specificTermCodesCount = specificTermCodes.Count;
                for (int i = 0; i < specificTermCodesCount; i++)
                {
                    termCodes.Add(specificTermCodes[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                termCodes.Add(CreateValidEntities.TermCode(i + specificTermCodesCount + 1));
            }

            var totalCount = termCodes.Count;
            for (int i = 0; i < totalCount; i++)
            {
                termCodes[i].SetIdTo((i + 1).ToString());
                //int i1 = i;
                //repository.OfType<State>()
                //    .Expect(a => a.GetNullableById(i1 + 1))
                //    .Return(TermCode[i])
                //    .Repeat
                //    .Any();
            }
            //State is not an Int Id, if I need to fake this, I'll need to pass a different repository
            //repository.OfType<TermCode>().Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            termCodeRepository.Expect(a => a.Queryable).Return(termCodes.AsQueryable()).Repeat.Any();
            termCodeRepository.Expect(a => a.GetAll()).Return(termCodes).Repeat.Any();
        }

        public static void FakeMajors(int count, IRepositoryWithTypedId<MajorCode, string> majorRepository)
        {
            var majorCodes = new List<MajorCode>();
            FakeMajors(count, majorRepository, majorCodes);
        }

        public static void FakeMajors(int count, IRepositoryWithTypedId<MajorCode, string> majorRepository, List<MajorCode> specificMajorCodes)
        {
            var majorCodes = new List<MajorCode>();
            var specificMajorCodesCount = 0;
            if (specificMajorCodes != null)
            {
                specificMajorCodesCount = specificMajorCodes.Count;
                for (int i = 0; i < specificMajorCodesCount; i++)
                {
                    majorCodes.Add(specificMajorCodes[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                majorCodes.Add(CreateValidEntities.MajorCode(i + specificMajorCodesCount + 1));
            }

            var totalCount = majorCodes.Count;
            for (int i = 0; i < totalCount; i++)
            {
                majorCodes[i].SetIdTo((i + 1).ToString());
                int i1 = i;
                majorRepository
                    .Expect(a => a.GetNullableById((i1 + 1).ToString()))
                    .Return(majorCodes[i])
                    .Repeat
                    .Any();
            }
            majorRepository.Expect(a => a.GetNullableById((totalCount + 1).ToString())).Return(null).Repeat.Any();
            majorRepository.Expect(a => a.Queryable).Return(majorCodes.AsQueryable()).Repeat.Any();
            majorRepository.Expect(a => a.GetAll()).Return(majorCodes).Repeat.Any();
        }

        public static void FakeRegistrationParticipation(int count, IRepository<RegistrationParticipation> repository)
        {
            var registrationParticipations = new List<RegistrationParticipation>();
            FakeRegistrationParticipation(count, repository, registrationParticipations);
        }


        public static void FakeRegistrationParticipation(int count, IRepository<RegistrationParticipation> repository, List<RegistrationParticipation> specificregistrationParticipations)
        {
            var registrationParticipations = new List<RegistrationParticipation>();
            var specificspecificregistrationParticipationsCount = 0;
            if (specificregistrationParticipations != null)
            {
                specificspecificregistrationParticipationsCount = specificregistrationParticipations.Count;
                for (int i = 0; i < specificspecificregistrationParticipationsCount; i++)
                {
                    registrationParticipations.Add(specificregistrationParticipations[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(i + specificspecificregistrationParticipationsCount + 1));
            }

            var totalCount = registrationParticipations.Count;
            for (int i = 0; i < totalCount; i++)
            {
                registrationParticipations[i].SetIdTo(i + 1);
                int i1 = i;
                repository
                    .Expect(a => a.GetNullableById(i1 + 1))
                    .Return(registrationParticipations[i])
                    .Repeat
                    .Any();
            }
            repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.Expect(a => a.Queryable).Return(registrationParticipations.AsQueryable()).Repeat.Any();
            repository.Expect(a => a.GetAll()).Return(registrationParticipations).Repeat.Any();
        }

        public static void FakevTermCode(int count, IRepository<vTermCode> termCodeRepository)
        {
            var termCodes = new List<vTermCode>();
            FakevTermCode(count, termCodeRepository, termCodes);
        }


        public static void FakevTermCode(int count, IRepository<vTermCode> termCodeRepository, List<vTermCode> specificTermCodes)
        {
            var termCodes = new List<vTermCode>();
            var specificTermCodesCount = 0;
            if (specificTermCodes != null)
            {
                specificTermCodesCount = specificTermCodes.Count;
                for (int i = 0; i < specificTermCodesCount; i++)
                {
                    termCodes.Add(specificTermCodes[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                termCodes.Add(CreateValidEntities.vTermCode(i + specificTermCodesCount + 1));
                termCodes[i + specificTermCodesCount].EndDate = DateTime.Now.AddDays((-2 + i)).Date;
            }

            var totalCount = termCodes.Count;
            for (int i = 0; i < totalCount; i++)
            {
                termCodes[i].SetIdTo((i + 1).ToString());
                //int i1 = i;
                //repository.OfType<State>()
                //    .Expect(a => a.GetNullableById(i1 + 1))
                //    .Return(TermCode[i])
                //    .Repeat
                //    .Any();
            }
            //State is not an Int Id, if I need to fake this, I'll need to pass a different repository
            //repository.OfType<TermCode>().Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            termCodeRepository.Expect(a => a.Queryable).Return(termCodes.AsQueryable()).Repeat.Any();
            termCodeRepository.Expect(a => a.GetAll()).Return(termCodes).Repeat.Any();
        }

        public static void FakeSpecialNeeds(int count, IRepository<SpecialNeed> repository)
        {
            var specialNeeds = new List<SpecialNeed>();
            FakeSpecialNeeds(count, repository, specialNeeds);
        }


        public static void FakeSpecialNeeds(int count, IRepository<SpecialNeed> repository, List<SpecialNeed> specificSpecialNeeds)
        {
            var specialNeeds = new List<SpecialNeed>();
            var specificSpecialNeedsCount = 0;
            if (specificSpecialNeeds != null)
            {
                specificSpecialNeedsCount = specificSpecialNeeds.Count;
                for (int i = 0; i < specificSpecialNeedsCount; i++)
                {
                    specialNeeds.Add(specificSpecialNeeds[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                specialNeeds.Add(CreateValidEntities.SpecialNeed(i + specificSpecialNeedsCount + 1));
            }

            var totalCount = specialNeeds.Count;
            for (int i = 0; i < totalCount; i++)
            {
                specialNeeds[i].SetIdTo(i + 1);
                int i1 = i;
                repository
                    .Expect(a => a.GetNullableById(i1 + 1))
                    .Return(specialNeeds[i])
                    .Repeat
                    .Any();
            }
            repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.Expect(a => a.Queryable).Return(specialNeeds.AsQueryable()).Repeat.Any();
            repository.Expect(a => a.GetAll()).Return(specialNeeds).Repeat.Any();
        }
    }
}
