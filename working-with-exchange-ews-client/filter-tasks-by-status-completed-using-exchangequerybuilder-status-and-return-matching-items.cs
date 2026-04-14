using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService; // for IEWSClient
using Aspose.Email.Clients.Exchange; // for ExchangeTaskStatus

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                // Replace with actual server URL and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to EWS: {ex.Message}");
                return;
            }

            using (client)
            {
                // Build query to filter tasks with status Completed
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                queryBuilder.TaskStatus.Equals(ExchangeTaskStatus.Completed);
                MailQuery query = queryBuilder.GetQuery();

                // Retrieve tasks (default tasks folder) matching the query
                TaskCollection tasks = client.ListTasks(null, query);

                // Output matching tasks
                foreach (ExchangeTask task in tasks)
                {
                    Console.WriteLine($"Subject: {task.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
