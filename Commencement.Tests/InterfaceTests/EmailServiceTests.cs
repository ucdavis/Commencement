using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Tests.InterfaceTests
{
    [TestClass]
    public class EmailServiceTests
    {
        public IRepository<Template> TemplateRepository;
        public IRepository<EmailQueue> EmailQueueRepository;
        public ILetterGenerator LetterGenerator;
        public IEmailService EmailService;

        public EmailServiceTests()
        {
            TemplateRepository = MockRepository.GenerateStub<IRepository<Template>>();
            EmailQueueRepository = MockRepository.GenerateStub<IRepository<EmailQueue>>();
            LetterGenerator = MockRepository.GenerateStub<ILetterGenerator>();
            EmailService = new EmailService(TemplateRepository, EmailQueueRepository, LetterGenerator);
        }



        [TestMethod]
        public void TestQueueRegistrationConfirmation1()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            var registrationParticipation = CreateValidEntities.RegistrationParticipation(1);
            registrationParticipation.Ceremony = CreateValidEntities.Ceremony(1);
            registrationParticipation.Registration = registration;
            var templates = new List<Template>();
            for (int i = 0; i < 3; i++)
            {
                templates.Add(CreateValidEntities.Template(i+1));
                templates[i].TemplateType = CreateValidEntities.TemplateType(i + 1);
            }

            templates[2].TemplateType.Name = StaticValues.Template_RegistrationConfirmation;

            registrationParticipation.Ceremony.Templates = templates;
            
            registration.RegistrationParticipations.Add(registrationParticipation);
            registration.Student = CreateValidEntities.Student(7);
            LetterGenerator.Expect(a => a.GenerateRegistrationConfirmation(Arg<RegistrationParticipation>.Is.Anything, Arg<Template>.Is.Anything)).Return("Body Text").Repeat.Any();
            EmailQueueRepository.Expect(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything));
            #endregion Arrange

            #region Act
            EmailService.QueueRegistrationConfirmation(registration);            
            #endregion Act

            #region Assert
            EmailQueueRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything));
            var args =(EmailQueue)EmailQueueRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual("Pidm7", args.Student.Pidm);
            Assert.AreEqual("Subject3", args.Template.Subject);
            Assert.AreEqual("Registration Confirmation", args.Template.TemplateType.Name);
            Assert.AreEqual("Body Text", args.Body);
            Assert.IsFalse(args.Immediate);
            #endregion Assert		
        }

        [TestMethod] //Task 237
        public void TestQueueRegistrationConfirmation2()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            var registrationParticipation = CreateValidEntities.RegistrationParticipation(1);
            registrationParticipation.Ceremony = CreateValidEntities.Ceremony(1);
            registrationParticipation.Registration = registration;
            var templates = new List<Template>();
            for (int i = 0; i < 3; i++)
            {
                templates.Add(CreateValidEntities.Template(i + 1));
                templates[i].TemplateType = CreateValidEntities.TemplateType(i + 1);
            }

            templates[2].TemplateType.Name = StaticValues.Template_RegistrationConfirmation;

            registrationParticipation.Ceremony.Templates = templates;

            registration.RegistrationParticipations.Add(registrationParticipation);

            //Second Registration Participation
            registrationParticipation = CreateValidEntities.RegistrationParticipation(2);
            registrationParticipation.Ceremony = CreateValidEntities.Ceremony(3);
            registrationParticipation.Registration = registration;
            templates = new List<Template>();
            for (int i = 3; i < 7; i++)
            {
                templates.Add(CreateValidEntities.Template(i + 1));
                templates[i-3].TemplateType = CreateValidEntities.TemplateType(i + 1);
            }

            templates[2].TemplateType.Name = StaticValues.Template_RegistrationConfirmation;

            registrationParticipation.Ceremony.Templates = templates;

            registration.RegistrationParticipations.Add(registrationParticipation);



            registration.Student = CreateValidEntities.Student(7);
            LetterGenerator.Expect(a => a.GenerateRegistrationConfirmation(Arg<RegistrationParticipation>.Is.Anything, Arg<Template>.Is.Anything)).Return("Body Text").Repeat.Any();
            EmailQueueRepository.Expect(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything));
            #endregion Arrange

            #region Act
            EmailService.QueueRegistrationConfirmation(registration);
            #endregion Act

            #region Assert
            EmailQueueRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything), x => x.Repeat.Times(2));
            var args = EmailQueueRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<EmailQueue>.Is.Anything));
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Count());

            var arg1 = (EmailQueue)args[0][0];
            var arg2 = (EmailQueue)args[1][0];


            Assert.AreEqual("Pidm7", arg1.Student.Pidm);
            Assert.AreEqual("Subject3", arg1.Template.Subject);
            Assert.AreEqual("Registration Confirmation", arg1.Template.TemplateType.Name);
            Assert.AreEqual("Body Text", arg1.Body);
            Assert.IsFalse(arg1.Immediate);

            Assert.AreEqual("Pidm7", arg2.Student.Pidm);
            Assert.AreEqual("Subject6", arg2.Template.Subject, "Task 237");
            Assert.AreEqual("Registration Confirmation", arg2.Template.TemplateType.Name);
            Assert.AreEqual("Body Text", arg2.Body);
            Assert.IsFalse(arg1.Immediate);
            #endregion Assert
        }
    }
}
