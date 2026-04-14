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
            // Define connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new task with a due date
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Sample Task";
                task.DueDate = DateTime.Now.AddDays(1); // due tomorrow

                // Create the task on the server and obtain its URI
                string taskUri = client.CreateTask(task);

                // Set the reminder to 15 minutes before the due date
                task.ReminderDate = task.DueDate.AddMinutes(-15);
                task.UniqueUri = taskUri; // associate the task object with its server URI

                // Update the task on the server with the new reminder setting
                client.UpdateTask(task);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
