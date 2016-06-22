using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Castle.Windsor;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.DataAnnotationsValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using Castle.MicroKernel.Registration;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;

namespace Commencement.Mvc
{
    internal static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);

            //container.AddComponent("studentService", typeof(IStudentService), typeof(DevStudentService));
            container.AddComponent("studentService", typeof(IStudentService), typeof(StudentService));
            container.AddComponent("emailService", typeof(IEmailService), typeof(EmailService));
            container.AddComponent("letterGenerator", typeof(ILetterGenerator), typeof(LetterGenerator));

            container.AddComponent("majorService", typeof(IMajorService), typeof(MajorService));
            container.AddComponent("auditInterceptor", typeof(NHibernate.IInterceptor), typeof(AuditInterceptor));
            container.AddComponent("principal", typeof(IPrincipal), typeof(WebPrincipal));
            container.AddComponent("ceremonyService", typeof(ICeremonyService), typeof(CeremonyService));
            container.AddComponent("userService", typeof(IUserService), typeof(UserService));
            container.AddComponent("registrationService", typeof(IRegistrationService), typeof(RegistrationService));
            container.AddComponent("petitionService", typeof(IPetitionService), typeof(PetitionService));
            container.AddComponent("errorService", typeof(IErrorService), typeof(ErrorService));
            container.AddComponent("registrationPopulator", typeof(IRegistrationPopulator), typeof(RegistrationPopulator));

            container.AddComponent("excelService", typeof(IExcelService), typeof(ExcelService));

            container.AddComponent("reportService", typeof(IReportService), typeof(ReportService));


            container.Register(Component.For<IValidator>().ImplementedBy<Validator>().Named("validator"));
            container.Register(Component.For<IDbContext>().ImplementedBy<DbContext>().Named("dbContext"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(RepositoryWithTypedId<,>)).Named("repositoryWithTypedId"));
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).Named("repositoryType"));
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>().Named("repository"));
        }
    }

    public class WebPrincipal : IPrincipal
    {
        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return HttpContext.Current.User.Identity; }
        }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

        #endregion
    }
}