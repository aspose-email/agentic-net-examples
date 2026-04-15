using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Set the timezone identifier for the client (applies to task creation)
                    client.TimezoneId = "Pacific Standard Time";

                    // Create a new task and set its properties
                    ExchangeTask task = new ExchangeTask();
                    task.Subject = "Sample Task";
                    task.Body = "Complete the report.";
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(2);
                    task.Status = ExchangeTaskStatus.NotStarted;

                    // Create the task on the server
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine("Aspose.Email.Calendar.Task created successfully. URI: " + taskUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during task operation: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
