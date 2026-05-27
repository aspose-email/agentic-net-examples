using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailEwsTaskReminder
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve all tasks from the default tasks folder
                        TaskCollection tasks = client.ListTasks();

                        foreach (Aspose.Email.Calendar.Task baseTask in tasks)
                        {
                            // Cast to ExchangeTask to access Exchange‑specific properties
                            Aspose.Email.Clients.Exchange.WebService.ExchangeTask task =
                                baseTask as Aspose.Email.Clients.Exchange.WebService.ExchangeTask;

                            if (task == null)
                                continue;

                            // Filter: High priority and InProgress status
                            if (task.Priority == Aspose.Email.MailPriority.High &&
                                task.Status == Aspose.Email.Clients.Exchange.WebService.ExchangeTaskStatus.InProgress)
                            {
                                // Build a reminder email
                                MailMessage reminder = new MailMessage();
                                reminder.From = "reminder@example.com";
                                reminder.To.Add(task.Organizer);
                                reminder.Subject = $"Reminder: {task.Subject}";
                                reminder.Body = $"Task \"{task.Subject}\" is in progress and marked as high priority. Please review.";

                                // Send the reminder
                                client.Send(reminder);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
