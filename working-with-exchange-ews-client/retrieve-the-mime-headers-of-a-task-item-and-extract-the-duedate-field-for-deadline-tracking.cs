using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            using (client)
            {
                // Obtain the Tasks folder URI
                string tasksFolderUri = client.MailboxInfo.TasksUri;

                // Retrieve tasks from the folder
                TaskCollection taskCollection = client.ListTasks(tasksFolderUri, 100, null, false);

                foreach (ExchangeTask exchangeTask in taskCollection)
                {
                    // Fetch the full message to access MIME headers
                    MailMessage message = client.FetchMessage(exchangeTask.UniqueUri);
                    using (message)
                    {
                        // Extract the 'DueDate' header
                        string dueDateHeader = message.Headers["DueDate"];
                        if (!string.IsNullOrEmpty(dueDateHeader) && DateTime.TryParse(dueDateHeader, out DateTime dueDate))
                        {
                            Console.WriteLine($"Task Subject: {exchangeTask.Subject}");
                            Console.WriteLine($"Due Date (from header): {dueDate}");
                        }
                        else
                        {
                            Console.WriteLine($"Task Subject: {exchangeTask.Subject}");
                            Console.WriteLine("Due Date header not found or invalid.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
