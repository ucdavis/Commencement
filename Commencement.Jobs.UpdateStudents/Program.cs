using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commencement.Core.Services;
using Commencement.Jobs.Common;
using Microsoft.Azure.WebJobs;
using Ninject;
using Dapper;
using Microsoft.WindowsAzure;

namespace Commencement.Jobs.UpdateStudents
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program : WebJobBase
    {
        private static IDbService _dbService;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main(string[] args)
        {
            var kernel = ConfigureServices();
            _dbService = kernel.Get<IDbService>();
            var jobHost = new JobHost();
            jobHost.Call(typeof(Program).GetMethod("UpdateStudentsFromDatamart"));
        }

        [NoAutomaticTrigger]
        public static void UpdateStudentsFromDatamart()
        {

            var runSproc = CloudConfigurationManager.GetSetting("RunUpdateStudentJob");

            //Don't execute unless email is turned on
            if (!string.Equals(runSproc, "Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Sproc Not run because RunUpdateStudentJob is not set to 'Yes'");
                return;
            }
            using (var connection = _dbService.GetConnection())
            {

                using (var ts = connection.BeginTransaction())
                {
                    //Update the db
                    connection.Execute(@"usp_ProcessStudentsMultiCollege", transaction: ts, commandType: CommandType.StoredProcedure);
                    connection.Execute(@"usp_ProcessMissingMajors", transaction: ts, commandType: CommandType.StoredProcedure);
                    ts.Commit();
                }
            }
        }
    }
}
