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
            // Placeholder mailbox URI and credentials
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Skip actual network call when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder mailbox URI detected. Skipping EWS operation.");
                return;
            }

            // Create asynchronous EWS client
            IAsyncEwsClient client;
            try
            {
                client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Build a new Exchange task
            ExchangeTask task = new ExchangeTask();
            task.Subject = "Sample Task";
            task.Body = "This is a sample task created via Aspose.Email.";
            task.DueDate = DateTime.Now.AddDays(3);
            task.Priority = MailPriority.High; // Use enum, not integer

            // Asynchronously create the task in the default task folder
            try
            {
                string taskUri = await client.CreateTaskAsync(task);
                Console.WriteLine($"Task created successfully. URI: {taskUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create task: {ex.Message}");
            }
            finally
            {
                if (client is IDisposable disposableClient)
                    disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
