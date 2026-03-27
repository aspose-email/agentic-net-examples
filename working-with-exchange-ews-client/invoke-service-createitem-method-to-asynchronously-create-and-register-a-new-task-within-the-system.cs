using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Initialize credentials and service URL (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the async EWS client
            using (IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(serviceUrl, credentials))
            {
                // Prepare a new task
                ExchangeTask task = new ExchangeTask
                {
                    Subject = "Sample Task",
                    Body = "Complete the sample task.",
                    DueDate = DateTime.Now.AddDays(3),
                    Priority = MailPriority.High
                };

                // Asynchronously create the task in the default task folder
                string taskUri = await client.CreateTaskAsync(task);

                Console.WriteLine("Task created successfully. URI: " + taskUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
