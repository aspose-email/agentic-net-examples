using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client with safety guard
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a high‑priority task
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Prepare Quarterly Report";
                task.Body = "Compile and review the quarterly financial report.";
                task.DueDate = DateTime.Now.AddDays(7); // due in 7 days
                task.ReminderDate = task.DueDate.AddDays(-1); // reminder one day before due
                task.Priority = MailPriority.High; // correct enum assignment

                // Assign the task to a team member
                task.Attendees.Add(new MailAddress("team.member@example.com"));

                // Create the task on the server
                string taskUri = client.CreateTask(task);
                Console.WriteLine("Aspose.Email.Calendar.Task created successfully. URI: " + taskUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
