using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Identifier of the task to be updated
                string taskId = "task-id";

                // Fetch the existing task from the server
                ExchangeTask task = client.FetchTask(taskId);
                if (task == null)
                {
                    Console.Error.WriteLine("Task not found.");
                    return;
                }

                // Prepare markdown checklist
                string checklist = "- [ ] Item 1\r\n- [ ] Item 2\r\n- [x] Completed Item";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Update the task body with the checklist
                task.Body = checklist;
                task.IsBodyHtml = false; // plain‑text markdown

                // Save the changes back to the server
                client.UpdateTask(task);

                Console.WriteLine("Task body updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
