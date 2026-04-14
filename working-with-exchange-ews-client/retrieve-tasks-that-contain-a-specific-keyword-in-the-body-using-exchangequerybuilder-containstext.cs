using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                string keyword = "important";
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                queryBuilder.Body.Contains(keyword);
                MailQuery query = queryBuilder.GetQuery();

                TaskCollection tasks = client.ListTasks("tasks", query);
                foreach (ExchangeTask task in tasks)
                {
                    Console.WriteLine($"Task Subject: {task.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
