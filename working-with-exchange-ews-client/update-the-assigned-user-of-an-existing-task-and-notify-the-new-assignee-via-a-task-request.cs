using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual credentials and service URL)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Unique URI of the existing task to be updated
                string taskUri = "https://exchange.example.com/EWS/Tasks/12345";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password" || taskUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the existing task
                ExchangeTask task = client.FetchTask(taskUri);
                if (task == null)
                {
                    Console.Error.WriteLine("Task not found.");
                    return;
                }

                // Update the assignee (add new attendee)
                task.Attendees.Add(new MailAddress("new.assignee@example.com"));

                // Apply the update on the server
                client.UpdateTask(task);

                // Send a task request to the new assignee
                task.Request();

                Console.WriteLine("Task updated and request sent to the new assignee.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
