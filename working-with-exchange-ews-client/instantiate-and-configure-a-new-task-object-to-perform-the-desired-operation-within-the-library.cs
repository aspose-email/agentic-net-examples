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
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create and configure a new task
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Sample Task";
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(2);
                task.Body = "This is a sample task created via Aspose.Email.";

                // Add the task to the mailbox
                client.CreateTask(task);
                Console.WriteLine("Task created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
