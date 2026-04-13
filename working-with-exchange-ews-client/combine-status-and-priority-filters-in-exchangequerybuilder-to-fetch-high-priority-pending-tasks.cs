using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with actual mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a query to find tasks with status NotStarted (pending)
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                MailQuery statusQuery = queryBuilder.TaskStatus.Equals(ExchangeTaskStatus.NotStarted);

                // Retrieve tasks matching the status query (no limit on number of items)
                TaskCollection tasks = client.ListTasks(null, 0, statusQuery);

                // Filter tasks with high priority and display them
                foreach (ExchangeTask task in tasks)
                {
                    if (task.Priority == MailPriority.High)
                    {
                        Console.WriteLine($"Subject: {task.Subject}");
                        Console.WriteLine($"Priority: {task.Priority}");
                        Console.WriteLine($"Status: {task.Status}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
