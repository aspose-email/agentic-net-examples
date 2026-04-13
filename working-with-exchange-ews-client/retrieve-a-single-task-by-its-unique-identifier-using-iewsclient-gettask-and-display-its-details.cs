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
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Unique identifier (URI) of the task to retrieve
                string taskUri = "https://exchange.example.com/EWS/Tasks/uniqueTaskId";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || taskUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the task from the server
                ExchangeTask task = client.FetchTask(taskUri);

                // Display task details
                Console.WriteLine("Subject: " + task.Subject);
                Console.WriteLine("Body: " + task.Body);
                Console.WriteLine("Start Date: " + task.StartDate);
                Console.WriteLine("Due Date: " + task.DueDate);
                Console.WriteLine("Status: " + task.Status);
                Console.WriteLine("Percent Complete: " + task.PercentComplete);
                Console.WriteLine("Priority: " + task.Priority);
                Console.WriteLine("Completion Date: " + task.CompletionDate);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
