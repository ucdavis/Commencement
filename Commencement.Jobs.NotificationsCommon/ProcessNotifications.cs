using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Commencement.Core.Services;
using Commencement.Jobs.Common.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Dapper;
using Serilog;
using SparkPost;
using Content = SparkPost.Content;

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

            var log = Log.ForContext("jobid", Guid.NewGuid());

            //Don't execute unless email is turned on
            if (!string.Equals(sendEmail, "Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("No emails sent because send-email is not set to 'Yes'");
                log.Information("No emails sent because send-email is not set to 'Yes'");
                return;
            }

            using (var connection = dbService.GetConnection())
            {
                List<dynamic> pending = null;
                try
                {
                    if (!immediate)
                    {
                        throw new Exception("Test Test Test");
                    }
                    pending = connection.Query(@"
                    select emailqueue.id as id, [subject], [body], students.email as sEmail, registrations.email as rEmail
		            from emailqueue
			            inner join Students on students.Id = EmailQueue.Student_Id
			            LEFT OUTER JOIN Registrations on registrations.id = EmailQueue.RegistrationId
		            where Pending = 1 and [immediate] = @immediate", new { immediate }, commandTimeout:300).ToList();
                }
                catch (Exception e)
                {
                    NotifyAdminOnError(immediate, e);

                    //re-throw it.
                    throw e;
                }


                if (pending.Any())
                {
                    log.Information("{count} pending emails found", pending.Count);
                }

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
                    emailTransmission.Options.Transactional = true;

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
                        log.Error(ex, ex.GetBaseException().ToString());
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
                log.Information("Sent: {successCount} Errors: {errorCount}", successCount, errorCount);

            }
        }

        private static void NotifyAdminOnError(bool immediate, Exception e)
        {
            if (!immediate)
            {
                //Email App dev
                try
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
                            Subject = "Commencement Job Error",
                            Html = string.Format("There was an exception with the Commencement Email Daily job {0}",
                                e.InnerException)
                        }
                    };
                    emailTransmission.Options.Transactional = true;

                    emailTransmission.Recipients.Add(new Recipient
                    {
                        Address = new Address {Email = "AppRequests@caes.ucdavis.edu"}
                    });
                    emailTransmission.Recipients.Add(new Recipient {Address = new Address {Email = "jsylvestre@ucdavis.edu"}});
                    var client = new Client(SparkPostApiKey);
                    client.Transmissions.Send(emailTransmission).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Exception Detected: {0}", ex.GetBaseException()));
                }
            }
        }
    }
}
