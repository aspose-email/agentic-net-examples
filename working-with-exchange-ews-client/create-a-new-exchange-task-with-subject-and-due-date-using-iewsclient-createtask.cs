using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

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
                try
                {
                    // Create a new task
                    ExchangeTask task = new ExchangeTask();
                    task.Subject = "Project Kickoff";
                    task.DueDate = DateTime.Now.AddDays(7);

                    // Create the task in the default task folder
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine("Aspose.Email.Calendar.Task created successfully. URI: " + taskUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while creating task: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
