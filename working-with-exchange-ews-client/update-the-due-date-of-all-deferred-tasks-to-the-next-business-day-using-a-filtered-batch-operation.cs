using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailEwsTaskUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                ICredentials credentials = new NetworkCredential("username", "password");

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Retrieve all tasks from the default tasks folder
                    TaskCollection tasks = client.ListTasks();

                    foreach (ExchangeTask task in tasks)
                    {
                        // Process only tasks that are deferred
                        if (task.Status == ExchangeTaskStatus.Deferred)
                        {
                            // Determine the next business day based on the current date
                            DateTime nextBusinessDay = GetNextBusinessDay(DateTime.Now);

                            // Update the due date of the task
                            task.DueDate = nextBusinessDay;

                            // Persist the changes back to the server
                            client.UpdateTask(task);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }

        // Returns the next business day (skipping Saturday and Sunday)
        private static DateTime GetNextBusinessDay(DateTime fromDate)
        {
            DateTime candidate = fromDate.AddDays(1);
            while (candidate.DayOfWeek == DayOfWeek.Saturday || candidate.DayOfWeek == DayOfWeek.Sunday)
            {
                candidate = candidate.AddDays(1);
            }
            // Preserve the time component of the original date if needed
            return new DateTime(candidate.Year, candidate.Month, candidate.Day, fromDate.Hour, fromDate.Minute, fromDate.Second);
        }
    }
}
