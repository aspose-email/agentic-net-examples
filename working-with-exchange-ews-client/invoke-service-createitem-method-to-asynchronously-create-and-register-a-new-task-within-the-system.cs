using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    // Author: Aspose.Email example for creating a task asynchronously via IAsyncEwsClient
    static async Task Main(string[] args)
    {
        try
        {
            // Prepare connection parameters
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("user@example.com", "password");

            // Create the asynchronous EWS client
            IAsyncEwsClient asyncClient = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            if (asyncClient == null)
            {
                Console.Error.WriteLine("Failed to create EWS client.");
                return;
            }

            // Ensure the client is disposed after use
            using (asyncClient as IDisposable)
            {
                // Build a new Exchange task
                ExchangeTask task = new ExchangeTask
                {
                    Subject = "Prepare project report",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(3),
                    Body = "Complete the quarterly project report and send it to the manager."
                };

                // Asynchronously create the task in the default Tasks folder
                string taskId = await asyncClient.CreateTaskAsync(task, cancellationToken: CancellationToken.None);

                Console.WriteLine($"Task created successfully. Task ID: {taskId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
