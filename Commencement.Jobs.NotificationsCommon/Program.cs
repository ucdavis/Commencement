using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commencement.Core.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Dapper;

namespace Commencement.Jobs.NotificationsCommon
{
    public static class ProcessNotifications
    {
        static readonly string SparkPostApiKey = CloudConfigurationManager.GetSetting("SparkPostApiKey");

        public static void ProcessEmails(IDbService dbService, bool immediate)
        {
            var sendEmail = CloudConfigurationManager.GetSetting("send-email");
            var testEmail = CloudConfigurationManager.GetSetting("send-test-emails");

            //Don't execute unless email is turned on
            if (!string.Equals(sendEmail, "Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("No emails sent because send-email is not set to 'Yes'");
                return;
            }

            using (var connection = dbService.GetConnection())
            {
                List<dynamic> pending = connection.Query(@"
                    select emailqueue.id, [subject], [body]
                        ,case 
				            when registrations.email is null then students.email
				            else students.email + ';' + registrations.email
				        end as emails
		            from emailqueue
			            inner join Students on students.Id = EmailQueue.Student_Id
			            LEFT OUTER JOIN Registrations on registrations.id = EmailQueue.RegistrationId
		            where Pending = 1 and [immediate] = @immediate").ToList();

                foreach (var email in pending)
                {
                    var subject = email.subject;

                }
            }
    }
}
