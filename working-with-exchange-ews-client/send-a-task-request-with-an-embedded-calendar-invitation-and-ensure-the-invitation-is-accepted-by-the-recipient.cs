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
            // Mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use using to ensure disposal
            using (client)
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("recipient@example.com"));

                // Create a task that includes a calendar invitation (start and due dates)
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Project Task with Meeting";
                task.Body = "Please complete the task and attend the meeting.";
                task.StartDate = DateTime.Now.AddDays(1);
                task.DueDate = DateTime.Now.AddDays(2);
                task.Attendees = attendees;

                // Send the task request (EWS sends it to attendees automatically)
                try
                {
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine($"Task request created. URI: {taskUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create task request: {ex.Message}");
                    return;
                }

                // Note: Acceptance of the task invitation is performed by the recipient's client.
                // This sample demonstrates creation and sending of the task request with an embedded calendar invitation.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
