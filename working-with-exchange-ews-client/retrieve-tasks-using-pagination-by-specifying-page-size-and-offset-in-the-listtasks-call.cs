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
            // Define connection settings (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Folder URI for tasks (default tasks folder)
                string tasksFolderUri = client.MailboxInfo.TasksUri;

                // Pagination parameters
                int pageSize = 10; // number of tasks per page
                int offset = 0;    // offset is simulated by skipping already processed items

                while (true)
                {
                    // Retrieve a page of tasks
                    TaskCollection tasksPage = client.ListTasks(tasksFolderUri, pageSize, null, false);

                    if (tasksPage == null || tasksPage.Count == 0)
                        break; // No more tasks

                    Console.WriteLine($"Page starting at offset {offset}:");

                    // Process each task in the current page
                    foreach (ExchangeTask task in tasksPage)
                    {
                        Console.WriteLine($"- Subject: {task.Subject}");
                        Console.WriteLine($"  Status : {task.Status}");
                        Console.WriteLine($"  Due    : {task.DueDate}");
                    }

                    // Simulate offset for next iteration (EWS ListTasks does not support offset directly)
                    offset += pageSize;

                    // In a real scenario, you would adjust the query to fetch the next set,
                    // e.g., by using a date filter or by tracking the last retrieved task's UniqueId.
                    // For demonstration, we break after one iteration to avoid an infinite loop.
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
