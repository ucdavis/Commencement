using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commencement.Core.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Dapper;
using SparkPost;

namespace Commencement.Jobs.NotificationsCommon
{
    public static class ProcessNotifications
    {
        static readonly string SparkPostApiKey = CloudConfigurationManager.GetSetting("SparkPostApiKey");

        public static void ProcessEmails(IDbService dbService, bool immediate)
        {
            var sendEmail = CloudConfigurationManager.GetSetting("send-email");
            var testEmail = CloudConfigurationManager.GetSetting("send-test-emails");
            var errorCount = 0;
            var successCount = 0;

            //Don't execute unless email is turned on
            if (!string.Equals(sendEmail, "Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("No emails sent because send-email is not set to 'Yes'");
                return;
            }

            using (var connection = dbService.GetConnection())
            {
                List<dynamic> pending = connection.Query(@"
                    select emailqueue.id as id, [subject], [body], students.email as sEmail, registrations.email as rEmail
		            from emailqueue
			            inner join Students on students.Id = EmailQueue.Student_Id
			            LEFT OUTER JOIN Registrations on registrations.id = EmailQueue.RegistrationId
		            where Pending = 1 and [immediate] = @immediate", new { immediate }).ToList();


                foreach (var email in pending)
                {
                    var emailTransmission = new Transmission
                    {
                        Content = new Content
                        {
                            From =
                                new Address
                                {
                                    Email = "noreply@commencement-notify.ucdavis.edu",                                    
                                    Name = "UCD Commencement Notification"
                                },
                            Subject = email.subject ,
                            Html = email.body
                        }
                    };

                    if (!string.IsNullOrWhiteSpace(testEmail))
                    {
                        emailTransmission.Recipients.Add(new Recipient { Address = new Address { Email = testEmail } });
                    }
                    else
                    {
                        emailTransmission.Recipients.Add(new Recipient { Address = new Address { Email = email.sEmail } });
                        if (!string.IsNullOrWhiteSpace(email.rEmail))
                        {
                            emailTransmission.Recipients.Add(new Recipient { Address = new Address { Email = email.rEmail } });
                        }
                    }
                    var client = new Client(SparkPostApiKey);
                    DateTime? sentDateTime = null;
                    try
                    {
                        client.Transmissions.Send(emailTransmission).Wait();
                        sentDateTime = DateTime.UtcNow; //TODO: Pacific time it?
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        //TODO: Logging.
                        Console.WriteLine(string.Format("Exception Detected: {0}", ex.GetBaseException()));
                        // Log.Error(ex, "There was a problem emailing {email}", email.sEmail);
                        //I don't think we care if there are a few problems...
                        errorCount++;
                    }

                    if (string.IsNullOrWhiteSpace(testEmail))
                    {
                        using (var ts = connection.BeginTransaction())
                        {
                            //Update the db
                            connection.Execute(@"
                            UPDATE EmailQueue
                            SET 
                                 [Pending] = 0
                                ,[SentDateTime] = @sentDateTime      
                            WHERE id = @id", new { sentDateTime, id = email.id }, ts);

                            ts.Commit();
                        }
                    }



                }

                Console.WriteLine(string.Format("Sent: {0} Errors: {1}", successCount, errorCount));

                if (errorCount > 20)
                {
                    //Send us a notification?
                }
            }
        }
    }
}
