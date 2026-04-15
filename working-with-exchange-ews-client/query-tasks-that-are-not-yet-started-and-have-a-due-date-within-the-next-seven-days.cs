using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new System.Net.NetworkCredential(username, password)))
            {
                // Retrieve all tasks from the default Tasks folder
                TaskCollection taskCollection = client.ListTasks();

                DateTime now = DateTime.Now;
                DateTime maxDueDate = now.AddDays(7);

                // Iterate through tasks and filter by status and due date
                foreach (ExchangeTask task in taskCollection)
                {
                    if (task.Status == ExchangeTaskStatus.NotStarted &&
                        task.DueDate >= now && task.DueDate <= maxDueDate)
                    {
                        Console.WriteLine($"Subject: {task.Subject}");
                        Console.WriteLine($"Due Date: {task.DueDate:d}");
                        Console.WriteLine();
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
