using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commencement.Core.Services;
using Commencement.Jobs.Common;
using Commencement.Jobs.Common.Logging;
using Commencement.Jobs.NotificationsCommon;
using Microsoft.Azure.WebJobs;
using Ninject;
using Serilog;

namespace Commencement.Jobs.EmailNotificationImmediate
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program : WebJobBase
    {
        private static IDbService _dbService;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main(string[] args)
        {
            LogHelper.ConfigureLogging();
            var kernel = ConfigureServices();
            _dbService = kernel.Get<IDbService>();
            var jobHost = new JobHost();
            jobHost.Call(typeof(Program).GetMethod("EmailNotificationImmediate"));
        }

        [NoAutomaticTrigger]
        public static void EmailNotificationImmediate()
        {
            Log.Information("About to process emails");
            ProcessNotifications.ProcessEmails(_dbService, true);
            Log.Information("Done process emails");
        }
    }
}
