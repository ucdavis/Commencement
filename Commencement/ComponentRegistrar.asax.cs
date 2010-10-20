using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Castle.Windsor;
using Commencement.Controllers.Services;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.NHibernateValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using Commencement.Controllers.Helpers;

namespace Commencement
{
    public static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            //Add your components here
            container.AddComponent("validator", typeof(IValidator), typeof(Validator));
            container.AddComponent("dbContext", typeof(IDbContext), typeof(DbContext));

#if DEBUG
            container.AddComponent("studentService", typeof(IStudentService), typeof(StudentService));
            container.AddComponent("emailService", typeof (IEmailService), typeof (DevEmailService));
#else
            container.AddComponent("studentService", typeof(IStudentService), typeof(StudentService));
            container.AddComponent("emailService", typeof (IEmailService), typeof (EmailService));
#endif
            container.AddComponent("majorService", typeof (IMajorService), typeof (MajorService));
            container.AddComponent("auditInterceptor", typeof (NHibernate.IInterceptor), typeof (AuditInterceptor));
            container.AddComponent("principal", typeof (IPrincipal), typeof (WebPrincipal));
            container.AddComponent("ceremonyService", typeof (ICeremonyService), typeof (CeremonyService));
            container.AddComponent("userService", typeof (IUserService), typeof (UserService));

            AddRepositoriesTo(container);
        }

        private static void AddRepositoriesTo(IWindsorContainer container)
        {
            container.AddComponent("repository", typeof(IRepository), typeof(Repository));
            container.AddComponent("genericRepository", typeof(IRepository<>), typeof(Repository<>));
            container.AddComponent("typedRepository", typeof(IRepositoryWithTypedId<,>),
                                   typeof(RepositoryWithTypedId<,>));
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