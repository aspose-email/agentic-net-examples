using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailTaskExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox URI and credentials (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Instantiate and configure a new Exchange task
                    Aspose.Email.Clients.Exchange.WebService.ExchangeTask task = new Aspose.Email.Clients.Exchange.WebService.ExchangeTask();
                    task.Subject = "Sample Task";
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(3);
                    task.Body = "This is a sample task created via Aspose.Email.";

                    // Create the task in the default tasks folder
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
}