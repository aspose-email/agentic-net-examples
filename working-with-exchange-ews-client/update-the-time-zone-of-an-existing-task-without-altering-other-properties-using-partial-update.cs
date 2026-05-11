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
            // Initialize EWS client (replace with actual server, username, and password)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // The unique URI of the task to be updated
                string taskUri = "https://exchange.example.com/EWS/Tasks/12345";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password" || taskUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the existing task
                ExchangeTask task = client.FetchTask(taskUri);
                if (task == null)
                {
                    Console.Error.WriteLine("Task not found.");
                    return;
                }

                // Update only the time zone related property (e.g., set StartDate as UTC)
                // This demonstrates a partial update; other properties remain unchanged.
                task.StartDate = new DateTime(2026, 5, 10, 9, 0, 0, DateTimeKind.Utc);

                // Apply the update
                client.UpdateTask(task);

                Console.WriteLine("Task time zone (StartDate) updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
