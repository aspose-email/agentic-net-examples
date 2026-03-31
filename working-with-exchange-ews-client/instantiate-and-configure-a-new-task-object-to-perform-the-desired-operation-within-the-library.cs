using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network call when placeholders are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Skipping EWS operation due to placeholder credentials.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Configure a new task
                ExchangeTask task = new ExchangeTask
                {
                    Subject = "Sample Task",
                    Body = "This is a sample task created via Aspose.Email EWS client.",
                    DueDate = DateTime.Now.AddDays(7),
                    IsBodyHtml = false,
                    Priority = MailPriority.Normal
                };

                // Create the task on the server
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
