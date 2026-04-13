using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
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

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Retrieve all tasks
                TaskCollection tasks = client.ListTasks();

                foreach (Aspose.Email.Calendar.Task task in tasks)
                {
                    bool hasNoAttachments = task.Attachments == null || task.Attachments.Count == 0;
                    bool isOlderThanSixMonths = task.DueDate < DateTime.Now.AddMonths(-6);

                    if (hasNoAttachments && isOlderThanSixMonths)
                    {
                        try
                        {
                            // Delete the task (move to Deleted Items)
                            client.DeleteItem(task.UniqueId, new DeletionOptions(DeletionType.MoveToDeletedItems));
                            Console.WriteLine($"Deleted task: {task.Subject}");
                        }
                        catch (Exception deleteEx)
                        {
                            Console.Error.WriteLine($"Failed to delete task '{task.Subject}': {deleteEx.Message}");
                        }
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
