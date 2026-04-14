using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

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
                // Replace with actual server URL and credentials
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (ewsUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                ICredentials credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(ewsUrl, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Create a new task
            ExchangeTask task = new ExchangeTask();
            task.Subject = "Prepare quarterly report";
            task.Body = "Compile and review the quarterly financial report.";
            task.DueDate = DateTime.Now.AddDays(7);
            task.ReminderDate = DateTime.Now.AddDays(6).AddHours(9); // reminder one day before due date

            // Assign the task to a colleague
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("colleague@example.com"));
            task.Attendees = attendees;

            // Create the task in the default Tasks folder
            try
            {
                string taskUri = client.CreateTask(task);
                Console.WriteLine($"Task created successfully. URI: {taskUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create task: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
