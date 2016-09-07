using Ninject.Modules;
using Commencement.Core.Services;

namespace Commencement.Jobs.Common
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbService>().To<DbService>().Named("dbService");
        }
    }
}
