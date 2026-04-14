using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Create a daily recurring task
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Daily Follow‑up";
                task.StartDate = DateTime.Today;
                task.DueDate = DateTime.Today.AddDays(1);
                task.Body = "Complete the daily report.";
                // Daily recurrence every 1 day
                task.RecurrencePattern = new DailyRecurrencePattern(task.StartDate, 1);

                try
                {
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine($"Task created. URI: {taskUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create task: {ex.Message}");
                    return;
                }

                // Retrieve tasks to verify recurrence instances
                try
                {
                    TaskCollection tasks = client.ListTasks();
                    Console.WriteLine("Tasks in the default task folder:");
                    foreach (Task t in tasks)
                    {
                        Console.WriteLine($"- Subject: {t.Subject}, Start: {t.StartDate:d}, Due: {t.DueDate:d}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list tasks: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
