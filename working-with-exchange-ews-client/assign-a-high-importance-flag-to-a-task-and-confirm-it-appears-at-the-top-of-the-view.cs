using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange mailbox URI and credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Create a new task with high importance.
                    ExchangeTask task = new ExchangeTask
                    {
                        Subject = "Prepare Quarterly Report",
                        Body = "Compile and review the quarterly financial report.",
                        DueDate = DateTime.Now.AddDays(7),
                        // Set the task priority to High using the MailPriority enum.
                        Priority = MailPriority.High
                    };

                    // Save the task to the default task folder.
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine($"Task created with URI: {taskUri}");

                    // Retrieve all tasks.
                    TaskCollection tasks = client.ListTasks();

                    // Order tasks so that high‑priority tasks appear first.
                    var orderedTasks = tasks.OrderByDescending(t => t.Priority);

                    Console.WriteLine("\nTasks (high priority first):");
                    foreach (Task t in orderedTasks)
                    {
                        Console.WriteLine($"- Subject: {t.Subject}, Priority: {t.Priority}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
