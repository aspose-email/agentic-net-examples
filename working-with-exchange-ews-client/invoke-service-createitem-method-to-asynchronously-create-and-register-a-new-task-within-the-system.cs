using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Define connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Initialize the async EWS client
            IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            if (client == null)
            {
                Console.Error.WriteLine("Failed to create EWS client.");
                return;
            }

            // Create a new Exchange task
            ExchangeTask task = new ExchangeTask
            {
                Subject = "Prepare project report",
                Body = "Complete the quarterly project report and submit it to management.",
                DueDate = DateTime.Now.AddDays(7),
                Priority = MailPriority.High // Use MailPriority enum, not an integer
            };

            // Asynchronously create the task in the default Tasks folder
            string taskUri = await client.CreateTaskAsync(task);
            Console.WriteLine($"Task created successfully. URI: {taskUri}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
