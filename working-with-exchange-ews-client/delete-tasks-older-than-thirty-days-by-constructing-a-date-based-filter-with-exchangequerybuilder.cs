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
            // Create and connect to the EWS client
            try
            {
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Retrieve all tasks from the default Tasks folder
                    TaskCollection tasks = client.ListTasks();

                    foreach (ExchangeTask task in tasks)
                    {
                        // Delete tasks whose due date is older than 30 days
                        if (task.DueDate < DateTime.Now.AddDays(-30))
                        {
                            try
                            {
                                client.DeleteItem(
                                    task.UniqueId,
                                    new DeletionOptions(DeletionType.MoveToDeletedItems));

                                Console.WriteLine($"Deleted task: {task.Subject}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine(
                                    $"Failed to delete task '{task.Subject}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS connection error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
